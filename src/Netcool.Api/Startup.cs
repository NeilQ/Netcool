using System.Reflection;
using System.Text;
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
using Netcool.Core.AspNetCore.ValueProviders;
using Netcool.Core.EfCore;
using Netcool.Core.Repositories;
using Netcool.Core.Services;
using Netcool.Core.Sessions;
using Netcool.Swashbuckle.AspNetCore;
using Serilog;

[assembly: ApiController]

namespace Netcool.Api
{
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
                    opt.ValueProviderFactories.Insert(0, new SnakeCaseQueryValueProviderFactory());
                    opt.ModelBinderProviders.RemoveType<DateTimeModelBinderProvider>(); // keeps use local datetime
                })
                .AddJsonOptions(o =>
                {
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

            // swagger
            services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<EnumDescriptionDocumentFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Netcool API", Version = "v1" });
                c.OperationFilter<FileUploadOperationFilter>();

                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
                //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Netcool.Core.xml"));
                //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Netcool.Api.Domain.xml"));
                //c.IncludeXmlComments(Assembly.GetAssembly(typeof(NetcoolDbContext)));
                c.IncludeAllXmlComments();

                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme,
                            },
                            Name = "Bearer"
                        },
                        new string[] { }
                    }
                });
            });

            // jwt
            var jwtOptions = Configuration.GetSection("Jwt").Get<JwtOptions>();
            services.Configure<JwtOptions>(Configuration.GetSection("Jwt"));
            var ipWhiteListOptions = Configuration.GetSection(nameof(IpWhitelistAuthenticationOptions))
                .Get<IpWhitelistAuthenticationOptions>();
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
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
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

            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // domain
            services.Configure<FileUploadOptions>(Configuration.GetSection("File"));
            services.AddScoped<IDbContext>(provider =>
                provider.GetService<NetcoolDbContext>()); // for UnitOfWork injection
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserSession, UserSession>();
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
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Netcool API V1"); });
        }
    }
}
