using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class UploadFileCustomers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UploadFiles",
                columns: table => new
                {
                    UploadedDocumentsId = table.Column<Guid>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    FileModifiedTime = table.Column<DateTime>(nullable: true),
                    FileExtension = table.Column<string>(nullable: true),
                    FileContents = table.Column<string>(nullable: true),
                    FileLength = table.Column<long>(nullable: true),
                    CustomerId = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    Createdby = table.Column<Guid>(nullable: false),
                    CheckSum = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadFiles", x => x.UploadedDocumentsId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploadFiles");
        }
    }
}
