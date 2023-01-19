using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class LeMigraticion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashedShortUrl",
                table: "UrlList");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HashedShortUrl",
                table: "UrlList",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
