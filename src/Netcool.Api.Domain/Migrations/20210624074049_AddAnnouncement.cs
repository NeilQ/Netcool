using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Netcool.Api.Domain.Migrations
{
    public partial class AddAnnouncement : Migration
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

            migrationBuilder.InsertData(
                table: "menus",
                columns: new[] { "id", "create_user_id", "delete_time", "delete_user_id", "display_name", "icon", "is_deleted", "level", "name", "notes", "order", "parent_id", "path", "route", "type", "update_user_id" },
                values: new object[] { 22, null, null, null, "公告", "notification", false, 2, "announcement", null, 3, 2, "/2/22", "/announcement", 1, null });

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 17,
                column: "permission_id",
                value: 30);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 18,
                column: "permission_id",
                value: 110);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 19,
                column: "permission_id",
                value: 31);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 20,
                column: "permission_id",
                value: 120);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 21,
                column: "permission_id",
                value: 121);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 22,
                column: "permission_id",
                value: 122);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 23,
                column: "permission_id",
                value: 123);

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "id", "code", "create_user_id", "delete_time", "delete_user_id", "is_deleted", "menu_id", "name", "notes", "type", "update_user_id" },
                values: new object[,]
                {
                    { 22, "announcement.view", null, null, null, false, 22, "公告", null, 0, null },
                    { 140, "announcement.create", null, null, null, false, 22, "公告新增", null, 1, null },
                    { 141, "announcement.update", null, null, null, false, 22, "公告修改", null, 1, null },
                    { 142, "announcement.delete", null, null, null, false, 22, "公告删除", null, 1, null },
                    { 143, "announcement.publish", null, null, null, false, 22, "公告发布", null, 1, null }
                });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "id", "create_user_id", "permission_id", "role_id" },
                values: new object[,]
                {
                    { 24, null, 22, 1 },
                    { 25, null, 140, 1 },
                    { 26, null, 141, 1 },
                    { 27, null, 142, 1 },
                    { 28, null, 143, 1 }
                });

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 12,
                column: "permission_id",
                value: 22);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 13,
                column: "permission_id",
                value: 140);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 14,
                column: "permission_id",
                value: 141);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 15,
                column: "permission_id",
                value: 142);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 16,
                column: "permission_id",
                value: 143);

            migrationBuilder.CreateIndex(
                name: "ix_user_announcements_announcement_id",
                table: "user_announcements",
                column: "announcement_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_announcements_user_id",
                table: "user_announcements",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_announcements");

            migrationBuilder.DropTable(
                name: "announcements");

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "menus",
                keyColumn: "id",
                keyValue: 22);

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

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 20,
                column: "permission_id",
                value: 130);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 21,
                column: "permission_id",
                value: 131);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 22,
                column: "permission_id",
                value: 132);

            migrationBuilder.UpdateData(
                table: "role_permissions",
                keyColumn: "id",
                keyValue: 23,
                column: "permission_id",
                value: 133);
        }
    }
}
