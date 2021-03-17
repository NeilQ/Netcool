using System.Collections.Generic;
using Netcool.Api.Domain.Configuration;
using Netcool.Api.Domain.Menus;
using Netcool.Api.Domain.Permissions;
using Netcool.Api.Domain.Roles;
using Netcool.Api.Domain.Users;
using Netcool.Core.AppSettings;
using Netcool.Core.Helpers;

namespace Netcool.Core
{
    public static class InitialEntities
    {
        public static List<AppConfiguration> GetInitialAppConfigurations()
        {
            return new List<AppConfiguration>()
            {
                new AppConfiguration(1, "User.DefaultPassword", "123456", "默认用户密码", AppConfigurationType.String, true)
            };
        }

        public static List<Menu> GetInitialMenus()
        {
            return new List<Menu>
            {
                new Menu(1, "dashboard", "首页", MenuType.Link, "/dashboard", "home", 1, 1, 0, "/1"),
                new Menu(2, "system", "系统设置", MenuType.Node, "/system", "setting", 1, 2, 0, "/2"),
                new Menu(20, "app-configuration", "应用配置", MenuType.Link, "/app-configuration", "setting", 2, 1, 2,
                    "/2/20"),
                new Menu(21, "organization", "组织", MenuType.Link, "/organization", "apartment", 2, 2, 2,
                    "/2/21"),
                new Menu(3, "auth", "权限管理", MenuType.Node, "/auth", "safety-certificate", 1, 3, 0, "/3"),
                new Menu(30, "menu", "菜单管理", MenuType.Link, "/menu", "menu", 2, 1, 3, "/3/30"),
                new Menu(31, "role", "角色管理", MenuType.Link, "/role", "usergroup-add", 2, 2, 3, "/3/31"),
                new Menu(32, "user", "用户管理", MenuType.Link, "/user", "user", 2, 3, 3, "/3/32"),
            };
        }

        public static List<Permission> GetInitialPermissions()
        {
            return new()
            {
                new Permission(1, "首页", "home.view", PermissionType.Menu, 1),
                new Permission(2, "系统", "system.view", PermissionType.Menu, 2),
                new Permission(3, "权限", "auth.view", PermissionType.Menu, 3),
                new Permission(20, "配置", "config.view", PermissionType.Menu, 20),
                new Permission(100, "配置新增", "config.create", PermissionType.Function, 20),
                new Permission(101, "配置修改", "config.update", PermissionType.Function, 20),
                new Permission(102, "配置删除", "config.delete", PermissionType.Function, 20),
                
                new Permission(21, "组织", "organization.view", PermissionType.Menu, 21),
                new Permission(103, "组织新增", "organization.create", PermissionType.Function, 21),
                new Permission(104, "组织修改", "organization.update", PermissionType.Function, 21),
                new Permission(105, "组织删除", "organization.delete", PermissionType.Function, 21),
                
                new Permission(30, "菜单", "menu.view", PermissionType.Menu, 30),
                new Permission(110, "菜单修改", "menu.update", PermissionType.Function, 30),
                new Permission(31, "角色", "role.view", PermissionType.Menu, 31),
                new Permission(120, "角色新增", "role.create", PermissionType.Function, 31),
                new Permission(121, "角色修改", "role.update", PermissionType.Function, 31),
                new Permission(122, "角色删除", "role.delete", PermissionType.Function, 31),
                new Permission(123, "设置角色权限", "role.set-permissions", PermissionType.Function, 31),
                new Permission(32, "用户", "user.view", PermissionType.Menu, 32),
                new Permission(130, "用户新增", "user.create", PermissionType.Function, 32),
                new Permission(131, "用户修改", "user.update", PermissionType.Function, 32),
                new Permission(132, "用户删除", "user.delete", PermissionType.Function, 32),
                new Permission(133, "设置用户角色", "user.set-roles", PermissionType.Function, 32),
            };
        }

        public static List<User> GetInitialUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "admin",
                    DisplayName = "Admin",
                    IsActive = true,
                    Password = Encrypt.Md5By32("admin")
                }
            };
        }

        public static List<Role> GetInitialRoles()
        {
            return new List<Role> {new Role(1, "超级管理员")};
        }

        public static List<UserRole> GetInitialUserRoles()
        {
            var list = new List<UserRole>();
            var i = 1;
            foreach (var user in GetInitialUsers())
            {
                foreach (var role in GetInitialRoles())
                {
                    list.Add(new UserRole(i++, user.Id, role.Id));
                }
            }

            return list;
        }

        public static List<RolePermission> GetInitialRolePermissions()
        {
            var list = new List<RolePermission>();
            var i = 1;
            foreach (var role in GetInitialRoles())
            {
                foreach (var permission in GetInitialPermissions())
                {
                    list.Add(new RolePermission(i++, role.Id, permission.Id));
                }
            }

            return list;
        }
    }
}