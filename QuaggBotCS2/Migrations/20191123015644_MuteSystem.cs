using Microsoft.EntityFrameworkCore.Migrations;

namespace QuaggBotCS2.Migrations
{
    public partial class MuteSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Muted",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Muted",
                table: "Users");
        }
    }
}
