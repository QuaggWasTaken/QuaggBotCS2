using Microsoft.EntityFrameworkCore.Migrations;

namespace QuaggBotCS2.Migrations
{
    public partial class AddMutes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalMutes",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalMutes",
                table: "Users");
        }
    }
}
