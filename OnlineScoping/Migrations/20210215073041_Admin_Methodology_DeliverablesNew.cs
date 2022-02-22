using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class Admin_Methodology_DeliverablesNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin_Methodology_Deliverables",
                columns: table => new
                {
                    Methodology_DeliverablesId = table.Column<Guid>(nullable: false),
                    QuestionnaireId = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    ScopeTypeMethodolgy = table.Column<string>(nullable: true),
                    ScopeTypeMethodolgy_FileName = table.Column<string>(nullable: true),
                    ScopeTypeSampleReport = table.Column<string>(nullable: true),
                    ScopeTypeSampleReport_FileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin_Methodology_Deliverables", x => x.Methodology_DeliverablesId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin_Methodology_Deliverables");
        }
    }
}
