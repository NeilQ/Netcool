using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Netcool.Api.Domain.Migrations
{
    public partial class AddAppConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "app_configurations",
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
                    value = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_configurations", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_configurations");
        }
    }
}
