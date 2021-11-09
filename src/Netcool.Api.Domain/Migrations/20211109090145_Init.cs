using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Netcool.Api.Domain.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "announcements",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    body = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    notify_target_type = table.Column<int>(type: "integer", nullable: false),
                    create_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    create_user_id = table.Column<int>(type: "integer", nullable: true),
                    update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_user_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    delete_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    delete_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_announcements", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "app_configurations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    value = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    is_initial = table.Column<bool>(type: "boolean", nullable: false),
                    create_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    create_user_id = table.Column<int>(type: "integer", nullable: true),
                    update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_user_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    delete_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    delete_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_configurations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: true),
                    filename = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    create_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    create_user_id = table.Column<int>(type: "integer", nullable: true),
                    update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_user_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    delete_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    delete_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_files", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "menus",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    display_name = table.Column<string>(type: "text", nullable: true),
                    parent_id = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    route = table.Column<string>(type: "text", nullable: true),
                    icon = table.Column<string>(type: "text", nullable: true),
                    level = table.Column<int>(type: "integer", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    path = table.Column<string>(type: "text", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    create_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    create_user_id = table.Column<int>(type: "integer", nullable: true),
                    update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_user_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    delete_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    delete_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_menus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "organizations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    parent_id = table.Column<int>(type: "integer", nullable: true),
                    depth = table.Column<int>(type: "integer", nullable: false),
                    path = table.Column<string>(type: "text", nullable: true),
                    create_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    create_user_id = table.Column<int>(type: "integer", nullable: true),
                    update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_user_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    delete_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    delete_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizations", x => x.id);
                    table.ForeignKey(
                        name: "fk_organizations_organizations_parent_id",
                        column: x => x.parent_id,
                        principalTable: "organizations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    create_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    create_user_id = table.Column<int>(type: "integer", nullable: true),
                    update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_user_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    delete_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    delete_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_login_attempts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    login_name = table.Column<string>(type: "text", nullable: true),
                    success = table.Column<bool>(type: "boolean", nullable: false),
                    client_ip = table.Column<string>(type: "text", nullable: true),
                    client_name = table.Column<string>(type: "text", nullable: true),
                    browser_info = table.Column<string>(type: "text", nullable: true),
                    create_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    create_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_login_attempts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    menu_id = table.Column<int>(type: "integer", nullable: false),
                    create_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    create_user_id = table.Column<int>(type: "integer", nullable: true),
                    update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_user_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    delete_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    delete_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permissions", x => x.id);
                    table.ForeignKey(
                        name: "fk_permissions_menus_menu_id",
                        column: x => x.menu_id,
                        principalTable: "menus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    display_name = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    organization_id = table.Column<int>(type: "integer", nullable: true),
                    create_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    create_user_id = table.Column<int>(type: "integer", nullable: true),
                    update_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_user_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    delete_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    delete_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_organizations_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organizations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    permission_id = table.Column<int>(type: "integer", nullable: false),
                    create_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    create_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permissions", x => x.id);
                    table.ForeignKey(
                        name: "fk_role_permissions_permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_permissions_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_announcements",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    announcement_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false),
                    read_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_announcements", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_announcements_announcements_announcement_id",
                        column: x => x.announcement_id,
                        principalTable: "announcements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_announcements_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    create_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    create_user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "app_configurations",
                columns: new[] { "id", "create_time", "create_user_id", "delete_time", "delete_user_id", "description", "is_deleted", "is_initial", "name", "type", "update_time", "update_user_id", "value" },
                values: new object[] { 1, null, null, null, null, "默认用户密码", false, true, "User.DefaultPassword", 0, null, null, "123456" });

            migrationBuilder.InsertData(
                table: "menus",
                columns: new[] { "id", "create_time", "create_user_id", "delete_time", "delete_user_id", "display_name", "icon", "is_deleted", "level", "name", "notes", "order", "parent_id", "path", "route", "type", "update_time", "update_user_id" },
                values: new object[,]
                {
                    { 1, null, null, null, null, "首页", "home", false, 1, "dashboard", null, 1, 0, "/1", "/dashboard", 1, null, null },
                    { 2, null, null, null, null, "系统设置", "setting", false, 1, "system", null, 2, 0, "/2", "/system", 0, null, null },
                    { 3, null, null, null, null, "权限管理", "safety-certificate", false, 1, "auth", null, 3, 0, "/3", "/auth", 0, null, null },
                    { 20, null, null, null, null, "应用配置", "setting", false, 2, "app-configuration", null, 1, 2, "/2/20", "/app-configuration", 1, null, null },
                    { 21, null, null, null, null, "组织", "apartment", false, 2, "organization", null, 2, 2, "/2/21", "/organization", 1, null, null },
                    { 22, null, null, null, null, "公告", "notification", false, 2, "announcement", null, 3, 2, "/2/22", "/announcement", 1, null, null },
                    { 30, null, null, null, null, "菜单管理", "menu", false, 2, "menu", null, 1, 3, "/3/30", "/menu", 1, null, null },
                    { 31, null, null, null, null, "角色管理", "usergroup-add", false, 2, "role", null, 2, 3, "/3/31", "/role", 1, null, null },
                    { 32, null, null, null, null, "用户管理", "user", false, 2, "user", null, 3, 3, "/3/32", "/user", 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "create_time", "create_user_id", "delete_time", "delete_user_id", "is_deleted", "name", "notes", "update_time", "update_user_id" },
                values: new object[] { 1, null, null, null, null, false, "超级管理员", null, null, null });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "create_time", "create_user_id", "delete_time", "delete_user_id", "display_name", "email", "gender", "is_active", "is_deleted", "name", "organization_id", "password", "phone", "update_time", "update_user_id" },
                values: new object[] { 1, null, null, null, null, "Admin", null, 0, true, false, "admin", null, "21232F297A57A5A743894A0E4A801FC3", null, null, null });

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "id", "code", "create_time", "create_user_id", "delete_time", "delete_user_id", "is_deleted", "menu_id", "name", "notes", "type", "update_time", "update_user_id" },
                values: new object[,]
                {
                    { 1, "home.view", null, null, null, null, false, 1, "首页", null, 0, null, null },
                    { 2, "system.view", null, null, null, null, false, 2, "系统", null, 0, null, null },
                    { 3, "auth.view", null, null, null, null, false, 3, "权限", null, 0, null, null },
                    { 20, "config.view", null, null, null, null, false, 20, "配置", null, 0, null, null },
                    { 21, "organization.view", null, null, null, null, false, 21, "组织", null, 0, null, null },
                    { 22, "announcement.view", null, null, null, null, false, 22, "公告", null, 0, null, null },
                    { 30, "menu.view", null, null, null, null, false, 30, "菜单", null, 0, null, null },
                    { 31, "role.view", null, null, null, null, false, 31, "角色", null, 0, null, null },
                    { 32, "user.view", null, null, null, null, false, 32, "用户", null, 0, null, null },
                    { 100, "config.create", null, null, null, null, false, 20, "配置新增", null, 1, null, null },
                    { 101, "config.update", null, null, null, null, false, 20, "配置修改", null, 1, null, null },
                    { 102, "config.delete", null, null, null, null, false, 20, "配置删除", null, 1, null, null },
                    { 103, "organization.create", null, null, null, null, false, 21, "组织新增", null, 1, null, null },
                    { 104, "organization.update", null, null, null, null, false, 21, "组织修改", null, 1, null, null },
                    { 105, "organization.delete", null, null, null, null, false, 21, "组织删除", null, 1, null, null },
                    { 110, "menu.update", null, null, null, null, false, 30, "菜单修改", null, 1, null, null },
                    { 120, "role.create", null, null, null, null, false, 31, "角色新增", null, 1, null, null },
                    { 121, "role.update", null, null, null, null, false, 31, "角色修改", null, 1, null, null },
                    { 122, "role.delete", null, null, null, null, false, 31, "角色删除", null, 1, null, null },
                    { 123, "role.set-permissions", null, null, null, null, false, 31, "设置角色权限", null, 1, null, null },
                    { 130, "user.create", null, null, null, null, false, 32, "用户新增", null, 1, null, null },
                    { 131, "user.update", null, null, null, null, false, 32, "用户修改", null, 1, null, null },
                    { 132, "user.delete", null, null, null, null, false, 32, "用户删除", null, 1, null, null },
                    { 133, "user.set-roles", null, null, null, null, false, 32, "设置用户角色", null, 1, null, null },
                    { 140, "announcement.create", null, null, null, null, false, 22, "公告新增", null, 1, null, null },
                    { 141, "announcement.update", null, null, null, null, false, 22, "公告修改", null, 1, null, null },
                    { 142, "announcement.delete", null, null, null, null, false, 22, "公告删除", null, 1, null, null },
                    { 143, "announcement.publish", null, null, null, null, false, 22, "公告发布", null, 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "id", "create_time", "create_user_id", "role_id", "user_id" },
                values: new object[] { 1, null, null, 1, 1 });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "id", "create_time", "create_user_id", "permission_id", "role_id" },
                values: new object[,]
                {
                    { 1, null, null, 1, 1 },
                    { 2, null, null, 2, 1 },
                    { 3, null, null, 3, 1 },
                    { 4, null, null, 20, 1 },
                    { 5, null, null, 100, 1 },
                    { 6, null, null, 101, 1 },
                    { 7, null, null, 102, 1 },
                    { 8, null, null, 21, 1 },
                    { 9, null, null, 103, 1 },
                    { 10, null, null, 104, 1 },
                    { 11, null, null, 105, 1 },
                    { 12, null, null, 22, 1 },
                    { 13, null, null, 140, 1 },
                    { 14, null, null, 141, 1 },
                    { 15, null, null, 142, 1 },
                    { 16, null, null, 143, 1 },
                    { 17, null, null, 30, 1 },
                    { 18, null, null, 110, 1 },
                    { 19, null, null, 31, 1 },
                    { 20, null, null, 120, 1 },
                    { 21, null, null, 121, 1 },
                    { 22, null, null, 122, 1 },
                    { 23, null, null, 123, 1 },
                    { 24, null, null, 32, 1 },
                    { 25, null, null, 130, 1 },
                    { 26, null, null, 131, 1 },
                    { 27, null, null, 132, 1 },
                    { 28, null, null, 133, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "ix_files_filename",
                table: "files",
                column: "filename",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_organizations_parent_id",
                table: "organizations",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_permissions_menu_id",
                table: "permissions",
                column: "menu_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_permissions_permission_id",
                table: "role_permissions",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_permissions_role_id",
                table: "role_permissions",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_announcements_announcement_id",
                table: "user_announcements",
                column: "announcement_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_announcements_user_id",
                table: "user_announcements",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_user_id",
                table: "user_roles",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_organization_id",
                table: "users",
                column: "organization_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_configurations");

            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "user_announcements");

            migrationBuilder.DropTable(
                name: "user_login_attempts");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "announcements");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "menus");

            migrationBuilder.DropTable(
                name: "organizations");
        }
    }
}
