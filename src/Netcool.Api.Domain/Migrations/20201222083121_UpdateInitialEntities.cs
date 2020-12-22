using Microsoft.EntityFrameworkCore.Migrations;

namespace Netcool.Api.Domain.Migrations
{
    public partial class UpdateInitialEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 100,
                columns: new[] { "code", "name" },
                values: new object[] { "config.create", "配置新增" });

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "id", "code", "create_user_id", "delete_time", "delete_user_id", "is_deleted", "menu_id", "name", "notes", "type", "update_user_id" },
                values: new object[,]
                {
                    { 133, "user.set-roles", null, null, null, false, 32, "设置用户角色", null, 1, null },
                    { 101, "config.update", null, null, null, false, 20, "配置修改", null, 1, null },
                    { 102, "config.delete", null, null, null, false, 20, "配置删除", null, 1, null },
                    { 123, "role.set-permissions", null, null, null, false, 31, "设置角色权限", null, 1, null }
                });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "id", "create_user_id", "permission_id", "role_id" },
                values: new object[,]
                {
                    { 4, null, 20, 1 },
                    { 5, null, 100, 1 },
                    { 8, null, 30, 1 },
                    { 9, null, 110, 1 },
                    { 18, null, 132, 1 },
                    { 11, null, 120, 1 },
                    { 12, null, 121, 1 },
                    { 13, null, 122, 1 },
                    { 15, null, 32, 1 },
                    { 16, null, 130, 1 },
                    { 17, null, 131, 1 },
                    { 10, null, 31, 1 }
                });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "id", "create_user_id", "permission_id", "role_id" },
                values: new object[,]
                {
                    { 6, null, 101, 1 },
                    { 7, null, 102, 1 },
                    { 14, null, 123, 1 },
                    { 19, null, 133, 1 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 133);

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 100,
                columns: new[] { "code", "name" },
                values: new object[] { "config.update", "配置修改" });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "id", "create_user_id", "permission_id", "role_id" },
                values: new object[,]
                {
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
        }
    }
}
