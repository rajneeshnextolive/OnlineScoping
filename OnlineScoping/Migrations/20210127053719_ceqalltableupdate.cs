using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class ceqalltableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "CEQWorkstationAssessment",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "CEQSignature",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "CEQRemoteVulnScan",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "CEQQuestionResponse",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "CEQOrgDetails",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "CEQCloudSharedServicesAsses",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "CEQWorkstationAssessment");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "CEQSignature");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "CEQRemoteVulnScan");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "CEQQuestionResponse");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "CEQOrgDetails");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "CEQCloudSharedServicesAsses");
        }
    }
}
