using Microsoft.EntityFrameworkCore;
using Netcool.Api.Domain.Configuration;
using Netcool.Api.Domain.Menus;
using Netcool.Api.Domain.Permissions;
using Netcool.Api.Domain.Roles;
using Netcool.Api.Domain.Users;
using Netcool.Core.AppSettings;
using Netcool.Core.EfCore;
using Netcool.Core.Helpers;
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
        public DbSet<Files.File> Files { get; set; }

        public NetcoolDbContext(DbContextOptions<NetcoolDbContext> options, IUserSession userSession) : base(options,
            userSession)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>().Property(t => t.Id).HasIdentityOptions(startValue: 1000);
            modelBuilder.Entity<User>().Property(t => t.Id).HasIdentityOptions(startValue: 1000);
            modelBuilder.Entity<UserRole>().Property(t => t.Id).HasIdentityOptions(startValue: 1000);
            modelBuilder.Entity<Role>().Property(t => t.Id).HasIdentityOptions(startValue: 1000);
            modelBuilder.Entity<Permission>().Property(t => t.Id).HasIdentityOptions(startValue: 1000);
            modelBuilder.Entity<RolePermission>().Property(t => t.Id).HasIdentityOptions(startValue: 1000);
            modelBuilder.Entity<AppConfiguration>().Property(t => t.Id).HasIdentityOptions(startValue: 1000);
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
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Name = "admin",
                DisplayName = "Admin",
                IsActive = true,
                Password = Encrypt.Md5By32("admin")
            });
            modelBuilder.Entity<Role>().HasData(new Role(1, "超级管理员"));
            modelBuilder.Entity<UserRole>().HasData(new UserRole(1, 1, 1));

            SeedingAppConfiguration(modelBuilder);
            SeedingMenu(modelBuilder);
            SeedingPermissions(modelBuilder);
            SeedingRolePermissions(modelBuilder);
        }

        private static void SeedingAppConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfiguration>().HasData(new AppConfiguration
            {
                Id = 1,
                Name = "User.DefaultPassword",
                Value = "123456",
                Type = AppConfigurationType.String,
                Description = "默认用户密码",
                IsInitial = true
            });
        }

        private static void SeedingRolePermissions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission(1, 1, 1));
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission(2, 1, 2));
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission(3, 1, 3));

            modelBuilder.Entity<RolePermission>().HasData(new RolePermission(20, 1, 20));
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission(100, 1, 100));

            modelBuilder.Entity<RolePermission>().HasData(new RolePermission(30, 1, 30));
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission(110, 1, 110));

            modelBuilder.Entity<RolePermission>().HasData(new RolePermission(31, 1, 31));
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission(120, 1, 120));
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission(121, 1, 121));
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission(122, 1, 122));

            modelBuilder.Entity<RolePermission>().HasData(new RolePermission(32, 1, 32));
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission(130, 1, 130));
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission(131, 1, 131));
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission(132, 1, 132));
        }

        private static void SeedingPermissions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>().HasData(new Permission(1, "首页", "home.view", PermissionType.Menu, 1));
            modelBuilder.Entity<Permission>().HasData(new Permission(2, "系统", "system.view", PermissionType.Menu, 2));
            modelBuilder.Entity<Permission>().HasData(new Permission(3, "权限", "auth.view", PermissionType.Menu, 3));

            modelBuilder.Entity<Permission>().HasData(new Permission(20, "配置", "config.view", PermissionType.Menu, 20));
            modelBuilder.Entity<Permission>()
                .HasData(new Permission(100, "配置修改", "config.update", PermissionType.Function, 20));

            modelBuilder.Entity<Permission>().HasData(new Permission(30, "菜单", "menu.view", PermissionType.Menu, 30));
            modelBuilder.Entity<Permission>()
                .HasData(new Permission(110, "菜单修改", "menu.update", PermissionType.Function, 30));

            modelBuilder.Entity<Permission>().HasData(new Permission(31, "角色", "role.view", PermissionType.Menu, 31));
            modelBuilder.Entity<Permission>()
                .HasData(new Permission(120, "角色新增", "role.create", PermissionType.Function, 31));
            modelBuilder.Entity<Permission>()
                .HasData(new Permission(121, "角色修改", "role.update", PermissionType.Function, 31));
            modelBuilder.Entity<Permission>()
                .HasData(new Permission(122, "角色删除", "role.delete", PermissionType.Function, 31));

            modelBuilder.Entity<Permission>().HasData(new Permission(32, "用户", "user.view", PermissionType.Menu, 32));
            modelBuilder.Entity<Permission>()
                .HasData(new Permission(130, "用户新增", "user.create", PermissionType.Function, 32));
            modelBuilder.Entity<Permission>()
                .HasData(new Permission(131, "用户修改", "user.update", PermissionType.Function, 32));
            modelBuilder.Entity<Permission>()
                .HasData(new Permission(132, "用户删除", "user.delete", PermissionType.Function, 32));
        }

        private static void SeedingMenu(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>().HasData(
                new Menu(1, "dashboard", "首页", MenuType.Link, "/dashboard", "home", 1, 1, 0, "/1"));

            modelBuilder.Entity<Menu>().HasData(
                new Menu(2, "system", "系统设置", MenuType.Node, "/system", "setting", 1, 2, 0, "/2"));
            modelBuilder.Entity<Menu>().HasData(
                new Menu(20, "app-configuration", "应用配置", MenuType.Link, "/app-configuration", "setting", 2, 1, 2,
                    "/2/20"));

            modelBuilder.Entity<Menu>().HasData(
                new Menu(3, "auth", "权限管理", MenuType.Node, "/auth", "safety-certificate", 1, 3, 0, "/3"));
            modelBuilder.Entity<Menu>().HasData(
                new Menu(30, "menu", "菜单管理", MenuType.Link, "/menu", "menu", 2, 1, 3, "/3/30"));
            modelBuilder.Entity<Menu>().HasData(
                new Menu(31, "role", "角色管理", MenuType.Link, "/role", "usergroup-add", 2, 2, 3, "/3/31"));
            modelBuilder.Entity<Menu>().HasData(
                new Menu(32, "user", "用户管理", MenuType.Link, "/user", "user", 2, 3, 3, "/3/32"));
        }
    }
}