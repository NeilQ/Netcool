using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Netcool.Api.Domain.Configuration;
using Netcool.Api.Domain.EfCore;
using Netcool.Core;
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
            // Npgsql break changes for 6.0: https://www.npgsql.org/doc/types/datetime.html
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

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
                .ConfigureAppConfiguration(configBuilder =>
                {
                    var connectionString = configBuilder.Build().GetConnectionString("Database");
                    configBuilder.AddEfConfiguration(options => { options.UseNpgsql(connectionString); }, true);
                    configBuilder.AddJsonFileFromDirectory(Common.IsWindows ? "conf" : "/conf");
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
