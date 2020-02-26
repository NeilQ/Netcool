using Microsoft.EntityFrameworkCore.Migrations;

namespace Netcool.Api.Domain.Migrations
{
    public partial class ChangeMenuRoute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "menus",
                keyColumn: "id",
                keyValue: 1,
                column: "route",
                value: "/dashboard");

            migrationBuilder.UpdateData(
                table: "menus",
                keyColumn: "id",
                keyValue: 2,
                column: "route",
                value: "/system");

            migrationBuilder.UpdateData(
                table: "menus",
                keyColumn: "id",
                keyValue: 3,
                column: "route",
                value: "/auth");

            migrationBuilder.UpdateData(
                table: "menus",
                keyColumn: "id",
                keyValue: 20,
                column: "route",
                value: "/app-configuration");

            migrationBuilder.UpdateData(
                table: "menus",
                keyColumn: "id",
                keyValue: 30,
                column: "route",
                value: "/menu");

            migrationBuilder.UpdateData(
                table: "menus",
                keyColumn: "id",
                keyValue: 31,
                column: "route",
                value: "/role");

            migrationBuilder.UpdateData(
                table: "menus",
                keyColumn: "id",
                keyValue: 32,
                column: "route",
                value: "/user");

            migrationBuilder.CreateIndex(
                name: "ix_permissions_menu_id",
                table: "permissions",
                column: "menu_id");

            migrationBuilder.AddForeignKey(
                name: "fk_permissions_menus_menu_id",
                table: "permissions",
                column: "menu_id",
                principalTable: "menus",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_permissions_menus_menu_id",
                table: "permissions");

            migrationBuilder.DropIndex(
                name: "ix_permissions_menu_id",
                table: "permissions");

            migrationBuilder.UpdateData(
                table: "menus",
                keyColumn: "id",
                keyValue: 1,
                column: "route",
                value: "dashboard");

            migrationBuilder.UpdateData(
                table: "menus",
                keyColumn: "id",
                keyValue: 2,
                column: "route",
                value: "system");

            migrationBuilder.UpdateData(
                table: "menus",
                keyColumn: "id",
                keyValue: 3,
                column: "route",
                value: "auth");

            migrationBuilder.UpdateData(
                table: "menus",
                keyColumn: "id",
                keyValue: 20,
                column: "route",
                value: "app-configuration");

            migrationBuilder.UpdateData(
                table: "menus",
                keyColumn: "id",
                keyValue: 30,
                column: "route",
                value: "menu");

            migrationBuilder.UpdateData(
                table: "menus",
                keyColumn: "id",
                keyValue: 31,
                column: "route",
                value: "role");

            migrationBuilder.UpdateData(
                table: "menus",
                keyColumn: "id",
                keyValue: 32,
                column: "route",
                value: "user");
        }
    }
}
