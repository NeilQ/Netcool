using Microsoft.EntityFrameworkCore;
using Netcool.Api.Domain.Configuration;
using Netcool.Api.Domain.Menus;
using Netcool.Api.Domain.Organizations;
using Netcool.Api.Domain.Permissions;
using Netcool.Api.Domain.Roles;
using Netcool.Api.Domain.Users;
using Netcool.Core;
using Netcool.Core.Announcements;
using Netcool.Core.EfCore;
using Netcool.Core.Sessions;

namespace Netcool.Api.Domain.EfCore
{
    public class NetcoolDbContext : DbContextBase
    {
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<UserLoginAttempt> UserLoginAttempts { get; set; }
        public DbSet<AppConfiguration> AppConfigurations { get; set; }
        public DbSet<Files.File> Files { get; set; }

        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<UserAnnouncement> UserAnnouncements { get; set; }

        public NetcoolDbContext(DbContextOptions<NetcoolDbContext> options, IUserSession userSession) : base(options,
            userSession)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // postgresql
            modelBuilder.Entity<Organization>().Property(t => t.Id).HasIdentityOptions(startValue: 1000);
            modelBuilder.Entity<Menu>().Property(t => t.Id).HasIdentityOptions(startValue: 1000);
            modelBuilder.Entity<User>().Property(t => t.Id).HasIdentityOptions(startValue: 1000);
            modelBuilder.Entity<UserRole>().Property(t => t.Id).HasIdentityOptions(startValue: 1000);
            modelBuilder.Entity<Role>().Property(t => t.Id).HasIdentityOptions(startValue: 1000);
            modelBuilder.Entity<Permission>().Property(t => t.Id).HasIdentityOptions(startValue: 1000);
            modelBuilder.Entity<RolePermission>().Property(t => t.Id).HasIdentityOptions(startValue: 1000);
            modelBuilder.Entity<AppConfiguration>().Property(t => t.Id).HasIdentityOptions(startValue: 1000);

            // sql server
            /*
            modelBuilder.Entity<Organization>().Property(t => t.Id).UseIdentityColumn(1000);
            modelBuilder.Entity<Menu>().Property(t => t.Id).UseIdentityColumn(1000);
            modelBuilder.Entity<User>().Property(t => t.Id).UseIdentityColumn(1000);
            modelBuilder.Entity<UserRole>().Property(t => t.Id).UseIdentityColumn(1000);
            modelBuilder.Entity<Role>().Property(t => t.Id).UseIdentityColumn(1000);
            modelBuilder.Entity<Permission>().Property(t => t.Id).UseIdentityColumn(1000);
            modelBuilder.Entity<RolePermission>().Property(t => t.Id).UseIdentityColumn(1000);
            modelBuilder.Entity<AppConfiguration>().Property(t => t.Id).UseIdentityColumn(1000);
            */

            SeedingData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention();
            base.OnConfiguring(optionsBuilder);
        }

        private static void SeedingData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(InitialEntities.GetInitialUsers());
            modelBuilder.Entity<Role>().HasData(InitialEntities.GetInitialRoles());
            modelBuilder.Entity<UserRole>().HasData(InitialEntities.GetInitialUserRoles());
            modelBuilder.Entity<AppConfiguration>().HasData(InitialEntities.GetInitialAppConfigurations());
            modelBuilder.Entity<Menu>().HasData(InitialEntities.GetInitialMenus());
            modelBuilder.Entity<Permission>().HasData(InitialEntities.GetInitialPermissions());
            modelBuilder.Entity<RolePermission>().HasData(InitialEntities.GetInitialRolePermissions());
        }
    }
}
