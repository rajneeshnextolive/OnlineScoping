using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class resupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cost",
                table: "QuestionResponse",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Days",
                table: "QuestionResponse",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Requirement",
                table: "QuestionResponse",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "QuestionResponse");

            migrationBuilder.DropColumn(
                name: "Days",
                table: "QuestionResponse");

            migrationBuilder.DropColumn(
                name: "Requirement",
                table: "QuestionResponse");
        }
    }
}
