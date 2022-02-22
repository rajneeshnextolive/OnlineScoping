using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class isCustomerUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCustomerUpdate",
                table: "Customer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "QuestionnairDetails",
                columns: table => new
                {
                    QuestionnairDetailsId = table.Column<Guid>(nullable: false),
                    QuestionnaireId = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    ProjectName = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    ApplicationName = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionnairDetails", x => x.QuestionnairDetailsId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionnairDetails");

            migrationBuilder.DropColumn(
                name: "IsCustomerUpdate",
                table: "Customer");
        }
    }
}
