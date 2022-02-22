using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class tempusersales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TempUserName",
                table: "Sales",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TempUserName",
                table: "Sales");
        }
    }
}
