using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Netcool.Core.Sessions;

namespace Netcool.Api.Domain.EfCore
{
    public class NetcoolDbContextFactory : IDesignTimeDbContextFactory<NetcoolDbContext>

    {
        public NetcoolDbContext CreateDbContext(string[] args)
        {
            // Npgsql Break changes for 6.0: https://www.npgsql.org/doc/types/datetime.html
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            var optionsBuilder = new DbContextOptionsBuilder<NetcoolDbContext>();
            optionsBuilder.UseNpgsql(
                    "Server=127.0.0.1;Port=5413;Database=Netcool;User Id=postgres;Password=postgres;Enlist=true;")
                .UseSnakeCaseNamingConvention();

            return new NetcoolDbContext(optionsBuilder.Options, new NullCurrentUser());
        }
    }
}
