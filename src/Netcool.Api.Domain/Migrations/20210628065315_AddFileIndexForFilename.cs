using Microsoft.EntityFrameworkCore.Migrations;

namespace Netcool.Api.Domain.Migrations
{
    public partial class AddFileIndexForFilename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "index_files_filename",
                table: "files",
                column: "filename",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "index_files_filename",
                table: "files");
        }
    }
}
