using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GmailClient.Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddIsGoogleConectedProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGoogleConnected",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGoogleConnected",
                table: "AspNetUsers");
        }
    }
}
