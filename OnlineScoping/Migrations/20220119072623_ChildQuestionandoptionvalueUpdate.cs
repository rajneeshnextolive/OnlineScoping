using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class ChildQuestionandoptionvalueUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OptionValue",
                table: "Options",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "QuestionValue",
                table: "ChildQuestions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptionValue",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "QuestionValue",
                table: "ChildQuestions");
        }
    }
}
