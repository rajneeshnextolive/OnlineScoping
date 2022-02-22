using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class UpdateCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Client",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuestionnaireId",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RepliedDate",
                table: "Customer",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "SalesId",
                table: "Customer",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "SendDate",
                table: "Customer",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Client",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "QuestionnaireId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "RepliedDate",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "SalesId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "SendDate",
                table: "Customer");
        }
    }
}
