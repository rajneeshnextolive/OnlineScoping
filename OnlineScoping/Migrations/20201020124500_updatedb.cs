using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class updatedb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChildQuestionId",
                table: "QuestionResponse",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "Questionnaire",
                table: "Customer",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChildQuestionId",
                table: "QuestionResponse");

            migrationBuilder.DropColumn(
                name: "Questionnaire",
                table: "Customer");
        }
    }
}
