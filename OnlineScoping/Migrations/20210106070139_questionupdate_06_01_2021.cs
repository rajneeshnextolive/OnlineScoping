using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class questionupdate_06_01_2021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cost",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Days",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Requirement",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VariableName",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cost",
                table: "ChildQuestions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Days",
                table: "ChildQuestions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Requirement",
                table: "ChildQuestions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VariableName",
                table: "ChildQuestions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Days",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Requirement",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "VariableName",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "ChildQuestions");

            migrationBuilder.DropColumn(
                name: "Days",
                table: "ChildQuestions");

            migrationBuilder.DropColumn(
                name: "Requirement",
                table: "ChildQuestions");

            migrationBuilder.DropColumn(
                name: "VariableName",
                table: "ChildQuestions");
        }
    }
}
