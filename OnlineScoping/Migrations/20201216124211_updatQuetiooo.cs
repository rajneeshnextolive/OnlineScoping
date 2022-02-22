using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class updatQuetiooo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Response",
                table: "CEQQuestionResponse",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Weighting",
                table: "CEQQuestionResponse",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Response",
                table: "CEQQuestionResponse");

            migrationBuilder.DropColumn(
                name: "Weighting",
                table: "CEQQuestionResponse");
        }
    }
}
