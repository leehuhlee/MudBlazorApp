using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MudBlazorApp.Server.Migrations
{
    public partial class AddChatMessageUserProfilePictureUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromUserProfilePictureUrl",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromUserProfilePictureUrl",
                table: "ChatMessages");
        }
    }
}
