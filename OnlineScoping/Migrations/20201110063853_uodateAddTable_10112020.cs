using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class uodateAddTable_10112020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                table: "Sales",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Customer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerProposal",
                columns: table => new
                {
                    CustomerProposalId = table.Column<Guid>(nullable: false),
                    DocumentReference = table.Column<string>(nullable: true),
                    SalesId = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    QuestionnaireId = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProposal", x => x.CustomerProposalId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerProposal");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Address1",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Customer");
        }
    }
}
