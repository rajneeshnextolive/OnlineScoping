using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class changeprojectstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestStatus",
                table: "Projects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestStatus",
                table: "Projects");
        }
    }
}
