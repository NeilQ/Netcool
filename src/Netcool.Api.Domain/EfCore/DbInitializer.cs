using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Netcool.Api.Domain.Menus;
using Netcool.Api.Domain.Users;
using Netcool.Core.Helpers;

namespace Netcool.Api.Domain.EfCore
{
    public static class DbInitializer
    {
        public static void Initialize(NetcoolDbContext context)
        {
            InitializeUser(context);
            InitializeMenu(context);
        }

        private static void InitializeUser(NetcoolDbContext context)
        {
            if (DynamicQueryableExtensions.Any(context.Users)) return;
            context.Users.Add(new User
            {
                Name = "admin",
                DisplayName = "Admin",
                IsRoot = true,
                IsActive = true,
                Password = Encrypt.Md5By32("admin")
            });
            context.SaveChanges();
        }

        private static void InitializeMenu(NetcoolDbContext context)
        {
            var menus = context.Menus.AsQueryable().AsNoTracking().ToList();
            if (menus.All(t => t.Id != 1))
            {
                context.Menus.Add(new Menu
                {
                    Id = 1,
                    Name = "dashboard",
                    DisplayName = "首页",
                    Type = MenuType.Link,
                    Icon = "home",
                    Order = 1,
                    Route = "dashboard",
                    Path = "/1",
                    Level = 1
                });
            }

            if (menus.All(t => t.Id != 2))
            {
                context.Menus.Add(new Menu
                {
                    Id = 2,
                    Name = "system",
                    DisplayName = "系统设置",
                    Type = MenuType.Node,
                    Icon = "setting",
                    Order = 2,
                    Route = "system",
                    Path = "/2",
                    Level = 1
                });
            }

            if (menus.All(t => t.Id != 3))
            {
                context.Menus.Add(new Menu
                {
                    Id = 3,
                    Name = "auth",
                    DisplayName = "权限管理",
                    Type = MenuType.Node,
                    Icon = "safety-certificate",
                    Order = 3,
                    Route = "auth",
                    Path = "/3",
                    Level = 1
                });
            }

            if (menus.All(t => t.Id != 4))
            {
                context.Menus.Add(new Menu
                {
                    Id = 4,
                    Name = "app-configuration",
                    DisplayName = "应用配置",
                    Type = MenuType.Link,
                    Icon = "setting",
                    Order = 1,
                    Route = "system",
                    ParentId = 2, //系统设置
                    Path = "/2/4",
                    Level = 2
                });
            }

            if (menus.All(t => t.Id != 5))
            {
                context.Menus.Add(new Menu
                {
                    Id = 5,
                    Name = "menu",
                    DisplayName = "菜单管理",
                    Type = MenuType.Link,
                    Icon = "menu",
                    Order = 1,
                    Route = "menu",
                    ParentId = 3, //权限管理
                    Path = "/3/5",
                    Level = 2
                });
            }

            if (menus.All(t => t.Id != 6))
            {
                context.Menus.Add(new Menu
                {
                    Id = 6,
                    Name = "role",
                    DisplayName = "角色管理",
                    Type = MenuType.Link,
                    Icon = "usergroup-add",
                    Order = 2,
                    Route = "role",
                    ParentId = 3, //权限管理
                    Path = "/3/6",
                    Level = 2
                });
            }

            if (menus.All(t => t.Id != 7))
            {
                context.Menus.Add(new Menu
                {
                    Id = 7,
                    Name = "user",
                    DisplayName = "用户管理",
                    Type = MenuType.Link,
                    Icon = "user",
                    Order = 3,
                    Route = "user",
                    ParentId = 3, //权限管理
                    Path = "/3/7",
                    Level = 2
                });
            }

            if (menus.All(t => t.Id != 8))
            {
            }

            if (menus.All(t => t.Id != 9))
            {
            }

            if (menus.All(t => t.Id != 10))
            {
            }

            if (menus.All(t => t.Id != 11))
            {
            }

            context.SaveChanges();
        }
    }
}