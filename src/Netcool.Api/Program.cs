using System;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Netcool.Api.Domain.Configuration;
using Netcool.Api.Domain.EfCore;
using Serilog;
using Serilog.Events;

namespace Netcool.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "logs\\.log" : "/logs/.log";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    shared: true)
                .CreateLogger();

            var host = CreateHostBuilder(args).Build();

            CreateDbIfNotExists(host);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>().UseIISIntegration(); })
                .ConfigureAppConfiguration((hostContext, configBuilder) =>
                {
                    var connectionString = configBuilder.Build().GetConnectionString("Database");
                    configBuilder.AddEfConfiguration(options => { options.UseNpgsql(connectionString); }, true);
                })
                .UseSerilog();

        private static void CreateDbIfNotExists(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                using var context = services.GetRequiredService<NetcoolDbContext>();
                DbInitializer.Initialize(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }
    }
}