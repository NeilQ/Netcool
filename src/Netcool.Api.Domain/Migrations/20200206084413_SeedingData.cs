using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Netcool.Api.Domain.Migrations
{
    public partial class SeedingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_root",
                table: "users");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "users",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "user_roles",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "roles",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "role_permissions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "permissions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "menus",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'200', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.InsertData(
                table: "menus",
                columns: new[] { "id", "create_user_id", "delete_time", "delete_user_id", "display_name", "icon", "is_deleted", "level", "name", "notes", "order", "parent_id", "path", "route", "type", "update_user_id" },
                values: new object[,]
                {
                    { 1, null, null, null, "首页", "home", false, 1, "dashboard", null, 1, 0, "/1", "dashboard", 1, null },
                    { 2, null, null, null, "系统设置", "setting", false, 1, "system", null, 2, 0, "/2", "system", 0, null },
                    { 20, null, null, null, "应用配置", "setting", false, 2, "app-configuration", null, 1, 2, "/2/20", "app-configuration", 1, null },
                    { 3, null, null, null, "权限管理", "safety-certificate", false, 1, "auth", null, 3, 0, "/3", "auth", 0, null },
                    { 30, null, null, null, "菜单管理", "menu", false, 2, "menu", null, 1, 3, "/3/30", "menu", 1, null },
                    { 31, null, null, null, "角色管理", "usergroup-add", false, 2, "role", null, 2, 3, "/3/31", "role", 1, null },
                    { 32, null, null, null, "用户管理", "user", false, 2, "user", null, 3, 3, "/3/32", "user", 1, null }
                });

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "id", "code", "create_user_id", "delete_time", "delete_user_id", "is_deleted", "menu_id", "name", "notes", "type", "update_user_id" },
                values: new object[,]
                {
                    { 132, "user.delete", null, null, null, false, 32, "用户删除", null, 1, null },
                    { 131, "user.update", null, null, null, false, 32, "用户修改", null, 1, null },
                    { 130, "user.create", null, null, null, false, 32, "用户新增", null, 1, null },
                    { 32, "user.view", null, null, null, false, 32, "用户", null, 0, null },
                    { 122, "role.delete", null, null, null, false, 31, "角色删除", null, 1, null },
                    { 121, "role.update", null, null, null, false, 31, "角色修改", null, 1, null },
                    { 120, "role.create", null, null, null, false, 31, "角色新增", null, 1, null },
                    { 100, "config.update", null, null, null, false, 20, "配置修改", null, 1, null },
                    { 110, "menu.update", null, null, null, false, 30, "菜单修改", null, 1, null },
                    { 30, "menu.view", null, null, null, false, 30, "菜单", null, 0, null },
                    { 20, "config.view", null, null, null, false, 20, "配置", null, 0, null },
                    { 3, "auth.view", null, null, null, false, 3, "权限", null, 0, null },
                    { 2, "system.view", null, null, null, false, 2, "系统", null, 0, null },
                    { 1, "home.view", null, null, null, false, 1, "首页", null, 0, null },
                    { 31, "role.view", null, null, null, false, 31, "角色", null, 0, null }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "create_user_id", "delete_time", "delete_user_id", "is_deleted", "name", "notes", "update_user_id" },
                values: new object[] { 1, null, null, null, false, "超级管理员", null, null });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "create_user_id", "delete_time", "delete_user_id", "display_name", "email", "gender", "is_active", "is_deleted", "name", "password", "phone", "update_user_id" },
                values: new object[] { 1, null, null, null, "Admin", null, 0, true, false, "admin", "21232F297A57A5A743894A0E4A801FC3", null, null });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "id", "create_user_id", "permission_id", "role_id" },
                values: new object[,]
                {
                    { 1, null, 1, 1 },
                    { 2, null, 2, 1 },
                    { 3, null, 3, 1 },
                    { 20, null, 20, 1 },
                    { 100, null, 100, 1 },
                    { 30, null, 30, 1 },
                    { 110, null, 110, 1 },
                    { 31, null, 31, 1 },
                    { 120, null, 120, 1 },
                    { 121, null, 121, 1 },
                    { 122, null, 122, 1 },
                    { 32, null, 32, 1 },
                    { 130, null, 130, 1 },
                    { 131, null, 131, 1 },
                    { 132, null, 132, 1 }
                });

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "id", "create_user_id", "role_id", "user_id" },
                values: new object[] { 1, null, 1, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menus",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "menus",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "menus",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "menus",
                keyColumn: "id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "menus",
                keyColumn: "id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "menus",
                keyColumn: "id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "menus",
                keyColumn: "id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<bool>(
                name: "is_root",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "user_roles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "roles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "role_permissions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "permissions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "menus",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:IdentitySequenceOptions", "'200', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
