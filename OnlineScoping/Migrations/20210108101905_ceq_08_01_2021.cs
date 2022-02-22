using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class ceq_08_01_2021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cost",
                table: "CEQQuestions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Days",
                table: "CEQQuestions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Requirement",
                table: "CEQQuestions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VariableName",
                table: "CEQQuestions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "CEQQuestions");

            migrationBuilder.DropColumn(
                name: "Days",
                table: "CEQQuestions");

            migrationBuilder.DropColumn(
                name: "Requirement",
                table: "CEQQuestions");

            migrationBuilder.DropColumn(
                name: "VariableName",
                table: "CEQQuestions");
        }
    }
}
