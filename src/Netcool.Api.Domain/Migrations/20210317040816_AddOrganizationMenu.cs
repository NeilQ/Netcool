using Microsoft.EntityFrameworkCore.Migrations;

namespace Netcool.Api.Domain.Migrations
{
    public partial class AddOrganizationMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "menus",
                columns: new[] { "id", "create_user_id", "delete_time", "delete_user_id", "display_name", "icon", "is_deleted", "level", "name", "notes", "order", "parent_id", "path", "route", "type", "update_user_id" },
                values: new object[] { 21, null, null, null, "组织", "apartment", false, 2, "organization", null, 2, 2, "/2/21", "/organization", 1, null });

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 12,
                column: "permission_id",
                value: 30);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 13,
                column: "permission_id",
                value: 110);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 14,
                column: "permission_id",
                value: 31);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 15,
                column: "permission_id",
                value: 120);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 16,
                column: "permission_id",
                value: 121);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 17,
                column: "permission_id",
                value: 122);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 18,
                column: "permission_id",
                value: 123);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 19,
                column: "permission_id",
                value: 32);

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "id", "create_user_id", "permission_id", "role_id" },
                values: new object[,]
                {
                    { 20, null, 21, 1 },
                    { 21, null, 103, 1 },
                    { 22, null, 104, 1 },
                    { 23, null, 105, 1 }
                });

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "id", "code", "create_user_id", "delete_time", "delete_user_id", "is_deleted", "menu_id", "name", "notes", "type", "update_user_id" },
                values: new object[,]
                {
                    { 21, "organization.view", null, null, null, false, 21, "组织", null, 0, null },
                    { 103, "organization.create", null, null, null, false, 21, "组织新增", null, 1, null },
                    { 104, "organization.update", null, null, null, false, 21, "组织修改", null, 1, null },
                    { 105, "organization.delete", null, null, null, false, 21, "组织删除", null, 1, null }
                });

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 8,
                column: "permission_id",
                value: 21);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 9,
                column: "permission_id",
                value: 103);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 10,
                column: "permission_id",
                value: 104);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 11,
                column: "permission_id",
                value: 105);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "menus",
                keyColumn: "id",
                keyValue: 21);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 8,
                column: "permission_id",
                value: 30);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 9,
                column: "permission_id",
                value: 110);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 10,
                column: "permission_id",
                value: 31);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 11,
                column: "permission_id",
                value: 120);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 12,
                column: "permission_id",
                value: 121);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 13,
                column: "permission_id",
                value: 122);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 14,
                column: "permission_id",
                value: 123);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 15,
                column: "permission_id",
                value: 32);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 16,
                column: "permission_id",
                value: 130);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 17,
                column: "permission_id",
                value: 131);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 18,
                column: "permission_id",
                value: 132);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 19,
                column: "permission_id",
                value: 133);
        }
    }
}
