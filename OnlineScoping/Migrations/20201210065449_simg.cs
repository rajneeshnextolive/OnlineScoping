using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class simg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CEQSignature",
                columns: table => new
                {
                    CEQSignatureId = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    QuestionnaireId = table.Column<Guid>(nullable: false),
                    SignatureDate = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    JobTitle = table.Column<string>(nullable: true),
                    Nameoforganisation = table.Column<string>(nullable: true),
                    Nameofbusiness = table.Column<string>(nullable: true),
                    Cost = table.Column<int>(nullable: false),
                    Days = table.Column<int>(nullable: false),
                    Requirement = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CEQSignature", x => x.CEQSignatureId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CEQSignature");
        }
    }
}
