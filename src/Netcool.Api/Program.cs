using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Netcool.Api.Domain.Configuration;
using Netcool.Api.Domain.EfCore;
using Netcool.Core.Helpers;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace Netcool.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "logs\\.log" : "/logs/.log";
            var dbLogPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "logs\\db-.log" : "/logs/db-.log";
            const string formatTemplate =
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(Matching.FromSource("Microsoft.EntityFrameworkCore"))
                    .WriteTo.Console()
                    .WriteTo.File(dbLogPath, rollingInterval: RollingInterval.Day,
                        outputTemplate: formatTemplate,
                        shared: true))
                .WriteTo.Logger(lc => lc
                    .Filter.ByExcluding(Matching.FromSource("Microsoft.EntityFrameworkCore"))
                    .WriteTo.Console()
                    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day,
                        outputTemplate: formatTemplate,
                        shared: true))
                .CreateLogger();

            var host = CreateHostBuilder(args).Build();

            // CreateDbIfNotExists(host);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>().UseIISIntegration(); })
                .ConfigureAppConfiguration((hostContext, configBuilder) =>
                {
                    var connectionString = configBuilder.Build().GetConnectionString("Database");
                    configBuilder.AddEfConfiguration(options => { options.UseNpgsql(connectionString); }, true);

                    var confPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                        ? Path.Combine(Directory.GetCurrentDirectory(), "conf")
                        : "/conf";
                    if (Directory.Exists(confPath))
                    {
                        var files = Directory.GetFiles(confPath);
                        if (files == null || files.Length <= 0) return;
                        foreach (var file in files)
                        {
                            if (Path.GetExtension(file) == ".json")
                            {
                                configBuilder.AddJsonFile(file, optional: true, reloadOnChange: true);
                            }
                        }
                    }
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