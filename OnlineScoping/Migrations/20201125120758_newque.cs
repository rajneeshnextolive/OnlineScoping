using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class newque : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "res",
                table: "Options",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PCIQuestionnaire",
                columns: table => new
                {
                    PCIQuestionnaireId = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    QuestionnaireId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    IPAddress = table.Column<string>(nullable: true),
                    FQDN = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Ownership = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCIQuestionnaire", x => x.PCIQuestionnaireId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PCIQuestionnaire");

            migrationBuilder.DropColumn(
                name: "res",
                table: "Options");
        }
    }
}
