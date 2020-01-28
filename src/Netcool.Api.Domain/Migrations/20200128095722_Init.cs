using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Netcool.Api.Domain.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "menus",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    create_time = table.Column<DateTime>(nullable: true),
                    create_user_id = table.Column<int>(nullable: true),
                    update_time = table.Column<DateTime>(nullable: true),
                    update_user_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false),
                    delete_time = table.Column<DateTime>(nullable: true),
                    delete_user_id = table.Column<int>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    display_name = table.Column<string>(nullable: true),
                    parent_id = table.Column<int>(nullable: false),
                    route = table.Column<string>(nullable: true),
                    icon = table.Column<string>(nullable: true),
                    blank = table.Column<string>(nullable: true),
                    level = table.Column<int>(nullable: false),
                    order = table.Column<int>(nullable: false),
                    path = table.Column<string>(nullable: true),
                    notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_menus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    create_time = table.Column<DateTime>(nullable: true),
                    create_user_id = table.Column<int>(nullable: true),
                    update_time = table.Column<DateTime>(nullable: true),
                    update_user_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false),
                    delete_time = table.Column<DateTime>(nullable: true),
                    delete_user_id = table.Column<int>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    code = table.Column<string>(nullable: true),
                    notes = table.Column<string>(nullable: true),
                    type = table.Column<int>(nullable: false),
                    menu_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    create_time = table.Column<DateTime>(nullable: true),
                    create_user_id = table.Column<int>(nullable: true),
                    role_id = table.Column<int>(nullable: false),
                    permission_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    create_time = table.Column<DateTime>(nullable: true),
                    create_user_id = table.Column<int>(nullable: true),
                    update_time = table.Column<DateTime>(nullable: true),
                    update_user_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false),
                    delete_time = table.Column<DateTime>(nullable: true),
                    delete_user_id = table.Column<int>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    create_time = table.Column<DateTime>(nullable: true),
                    create_user_id = table.Column<int>(nullable: true),
                    user_id = table.Column<int>(nullable: false),
                    role_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    create_time = table.Column<DateTime>(nullable: true),
                    create_user_id = table.Column<int>(nullable: true),
                    update_time = table.Column<DateTime>(nullable: true),
                    update_user_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false),
                    delete_time = table.Column<DateTime>(nullable: true),
                    delete_user_id = table.Column<int>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    display_name = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    gender = table.Column<int>(nullable: false),
                    email = table.Column<string>(nullable: true),
                    phone = table.Column<string>(nullable: true),
                    is_active = table.Column<bool>(nullable: false),
                    is_root = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "menus");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
