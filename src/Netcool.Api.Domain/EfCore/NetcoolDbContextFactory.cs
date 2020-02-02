using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Netcool.Core.Sessions;

namespace Netcool.Api.Domain.EfCore
{
    public class NetcoolDbContextFactory : IDesignTimeDbContextFactory<NetcoolDbContext>

    {
        public NetcoolDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NetcoolDbContext>();
            optionsBuilder.UseNpgsql(
                    "Server=127.0.0.1;Port=5432;Database=Netcool;User Id=postgres;Password=postgres;Enlist=true;")
                .UseSnakeCaseNamingConvention();

            return new NetcoolDbContext(optionsBuilder.Options, new NullUserSession());
        }
    }
}