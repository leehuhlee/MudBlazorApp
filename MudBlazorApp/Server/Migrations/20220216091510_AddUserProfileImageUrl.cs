using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MudBlazorApp.Server.Migrations
{
    public partial class AddUserProfileImageUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureDataUrl",
                table: "Users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureDataUrl",
                table: "Users");
        }
    }
}
