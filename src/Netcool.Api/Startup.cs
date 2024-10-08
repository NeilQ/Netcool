using System.Reflection;
using System.Text;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Netcool.Api.Domain.Authorization;
using Netcool.Api.Domain.EfCore;
using Netcool.Api.Domain.Files;
using Netcool.Api.Domain.Repositories;
using Netcool.Core;
using Netcool.Core.AspNetCore;
using Netcool.Core.AspNetCore.Authentication.IpWhitelist;
using Netcool.Core.AspNetCore.Filters;
using Netcool.Core.AspNetCore.Json;
using Netcool.Core.AspNetCore.Middlewares;
using Netcool.Core.AspNetCore.ModelBinders;
using Netcool.Core.AspNetCore.ValueProviders;
using Netcool.Core.EfCore;
using Netcool.Core.Repositories;
using Netcool.Core.Services;
using Netcool.Core.Sessions;
using Netcool.Swashbuckle.AspNetCore;
using Serilog;
using File = Netcool.Api.Domain.Files.File;

[assembly: ApiController]

namespace Netcool.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(opt =>
            {
                opt.Filters.Add(new ValidateAttribute());
                opt.ModelBinderProviders.Insert(0, new LocalDateTimeModelBinderProvider());
                opt.ValueProviderFactories.Insert(0, new SnakeCaseQueryValueProviderFactory());
                opt.ModelBinderProviders.RemoveType<DateTimeModelBinderProvider>(); // keeps use local datetime
            })
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Converters.Add(new Int32Converter());
                o.JsonSerializerOptions.Converters.Add(new LocalDateTimeConverter()); //parse utc datetime to local;
                o.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
                o.JsonSerializerOptions.Converters.Add(new StringTrimConverter());
            });
        services.AddDbContext<NetcoolDbContext>(options =>
        {
            options.UseNpgsql(Configuration.GetConnectionString("Database"))
                .UseSnakeCaseNamingConvention();
        });

        services.AddHealthChecks();
        services.AddMemoryCache();

        services.AddMiniProfiler(options =>
        {
            options.RouteBasePath = "/profiler";
            options.EnableMvcFilterProfiling = false;
            options.EnableMvcViewProfiling = false;
            options.IgnoredPaths.AddIfNotContains("/swagger/");
        }).AddEntityFramework();

        // swagger
        services.AddSwaggerGen(c =>
        {
            c.DocumentFilter<EnumDescriptionDocumentFilter>();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Netcool API", Version = "v1" });
            c.OperationFilter<FileUploadOperationFilter>();

            c.IncludeAllXmlComments();
            c.AddJwtBearerSecurity();
        });

        // jwt
        var jwtOptions = Configuration.GetSection("Jwt").Get<JwtOptions>()!;
        services.Configure<JwtOptions>(Configuration.GetSection("Jwt"));
        var ipWhiteListOptions = Configuration.GetSection(nameof(IpWhitelistAuthenticationOptions))
            .Get<IpWhitelistAuthenticationOptions>()!;
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddIpWhitelist(o =>
            {
                o.Enable = ipWhiteListOptions.Enable;
                o.Ips = ipWhiteListOptions.Ips;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtOptions.ValidateIssuer,
                    ValidateAudience = jwtOptions.ValidateAudience,
                    ValidateLifetime = jwtOptions.ValidateLifetime,
                    ValidateIssuerSigningKey = jwtOptions.ValidateIssuerSigningKey,
                    ValidIssuer = jwtOptions.ValidIssuer,
                    ValidAudience = jwtOptions.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.IssuerSigningKey))
                };

                // We have to hook the OnMessageReceived event in order to
                // allow the JWT authentication handler to read the access
                // token from the query string when a WebSocket or
                // Server-Sent Events request comes in.
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },
                };
            });
        services.AddAuthorization(options =>
        {
            var defaultAuthorizationPolicyBuilder =
                new AuthorizationPolicyBuilder(IpWhitelistAuthenticationDefaults.AuthenticationScheme,
                    JwtBearerDefaults.AuthenticationScheme);
            defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
            options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
        });

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // mapper
        services.AddScoped<FileHostResolver>();
        services.AddScoped<FileUrlResolver>();
        var config = TypeAdapterConfig.GlobalSettings;
        config.NewConfig<File, FileDto>()
            .Map(dest => dest.Host, src => MapContext.Current.GetService<FileHostResolver>().Resolve())
            .Map(dest => dest.Url, src => MapContext.Current.GetService<FileUrlResolver>().Resolve(src));
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        // domain
        services.Configure<FileUploadOptions>(Configuration.GetSection("File"));
        services.AddScoped<IDbContext>(provider =>
            provider.GetService<NetcoolDbContext>()); // for UnitOfWork injection
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<IClientInfoProvider, HttpContextClientInfoProvider>();
        services.AddScoped(typeof(IRepository<>), typeof(CommonRepository<>));
        services.AddScoped(typeof(IRepository<,>), typeof(CommonRepository<,>));
        services.AddTransient<IServiceAggregator, ServiceAggregator>();
        services.AddDomainRepositoryTypes(Assembly.GetAssembly(typeof(NetcoolDbContext)), ServiceLifetime.Scoped);
        services.AddDomainServiceTypes(Assembly.GetAssembly(typeof(NetcoolDbContext)), ServiceLifetime.Scoped);
        // authorization
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMiniProfiler();
        }

        //app.UseHttpsRedirection();
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseSerilogRequestLogging();
        app.UseMiddleware(typeof(ErrorHandlingMiddleware));

        app.UseStaticFiles();
        app.UseUploadedStaticFiles(Configuration.GetSection("File").Get<FileUploadOptions>(), logger);

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            /* Add AllowAnonymousAttribute to all actions for dev env
            if (env.IsDevelopment())
                endpoints.MapControllers().WithMetadata(new AllowAnonymousAttribute());
            else
                endpoints.MapControllers();
                */
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
        });

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Netcool API V1");
            if (env.IsDevelopment())
            {
                c.InjectHeadContent(
                    @"<script async id=""mini-profiler"" src=""/profiler/includes.min.js?v=4.2.22+4563a9e1ab""
                          data-version=""4.2.22+4563a9e1ab"" data-path=""/profiler/"" 
                          data-current-id=""7a3d98bb-3968-41fb-8836-65f9923ee6eb""
                          data-ids=""7a3d98bb-3968-41fb-8836-65f9923ee6eb""
                          data-position=""Left"" data-scheme=""Light"" data-authorized=""true"" data-max-traces=""15""
                          data-toggle-shortcut=""Alt+P"" data-trivial-milliseconds=""2.0"" 
                          data-ignored-duplicate-execute-types=""Open,OpenAsync,Close,CloseAsync""></script>");
            }
        });
    }
}
