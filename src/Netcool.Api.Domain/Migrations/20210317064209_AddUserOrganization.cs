using Microsoft.EntityFrameworkCore.Migrations;

namespace Netcool.Api.Domain.Migrations
{
    public partial class AddUserOrganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "organization_id",
                table: "users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_organization_id",
                table: "users",
                column: "organization_id");

            migrationBuilder.AddForeignKey(
                name: "fk_users_organizations_organization_id",
                table: "users",
                column: "organization_id",
                principalTable: "organizations",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_organizations_organization_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_users_organization_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "organization_id",
                table: "users");
        }
    }
}
