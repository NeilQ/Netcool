using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Netcool.Api.Domain.EfCore;
using Netcool.Api.Domain.Repositories;
using Netcool.Api.Domain.Users;
using Netcool.Core;
using Netcool.Core.Authorization;
using Netcool.Core.Repositories;
using Netcool.Core.Services;
using Netcool.Core.Sessions;
using Netcool.Core.WebApi.Middlewares;
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
            services.AddControllers();
            services.AddDbContext<NetcoolDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Database"))
                    .UseSnakeCaseNamingConvention()
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddHealthChecks();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Netcool API", Version = "v1"});

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Netcool.Core.xml"));
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Netcool.Api.Domain.xml"));
            });

            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // domain
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IPermissionChecker, NullPermissionChecker>();
            services.AddScoped<IUserSession, UserSession>();
            services.AddTransient(typeof(IRepository<>), typeof(CommonRepository<>));
            services.AddTransient<IServiceAggregator, ServiceAggregator>();
            services.AddTransient<IUserService, UserService>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Netcool API V1"); });
        }
    }
}