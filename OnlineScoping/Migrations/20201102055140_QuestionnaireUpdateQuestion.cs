using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class QuestionnaireUpdateQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "QuestionnaireId",
                table: "Questions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "RowNumber",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SubTitle",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Responsetext",
                table: "QuestionResponse",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Questionnaire",
                columns: table => new
                {
                    QuestionnaireId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questionnaire", x => x.QuestionnaireId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Questionnaire");

            migrationBuilder.DropColumn(
                name: "QuestionnaireId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "RowNumber",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "SubTitle",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Responsetext",
                table: "QuestionResponse");
        }
    }
}
