using Microsoft.EntityFrameworkCore;
using Netcool.Api.Domain.Configuration;
using Netcool.Api.Domain.Menus;
using Netcool.Api.Domain.Permissions;
using Netcool.Api.Domain.Roles;
using Netcool.Api.Domain.Users;
using Netcool.Core.EfCore;
using Netcool.Core.Sessions;

namespace Netcool.Api.Domain.EfCore
{
    public class NetcoolDbContext : DbContextBase
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<UserLoginAttempt> UserLoginAttempts { get; set; }
        public DbSet<AppConfiguration> AppConfigurations { get; set; }

        public NetcoolDbContext(DbContextOptions<NetcoolDbContext> options, IUserSession userSession) : base(options,
            userSession)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>().Property(t => t.Id).HasIdentityOptions(startValue: 200);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention();
            base.OnConfiguring(optionsBuilder);
        }
    }
}