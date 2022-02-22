using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineScoping.Migrations
{
    public partial class ceq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CEQCloudSharedServicesAsses",
                columns: table => new
                {
                    CEQCloudSharedServicesAssesId = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    QuestionnaireId = table.Column<Guid>(nullable: false),
                    DescriptionOfService = table.Column<string>(nullable: true),
                    Supplier = table.Column<string>(nullable: true),
                    IndependentAuditStandards = table.Column<string>(nullable: true),
                    EvidenceOfcertification = table.Column<string>(nullable: true),
                    Cost = table.Column<int>(nullable: false),
                    Days = table.Column<int>(nullable: false),
                    Requirement = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CEQCloudSharedServicesAsses", x => x.CEQCloudSharedServicesAssesId);
                });

            migrationBuilder.CreateTable(
                name: "CEQOptions",
                columns: table => new
                {
                    CEQOptionId = table.Column<Guid>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    OptionText = table.Column<string>(nullable: true),
                    res = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CEQOptions", x => x.CEQOptionId);
                });

            migrationBuilder.CreateTable(
                name: "CEQOrgDetails",
                columns: table => new
                {
                    CEQOrgDetailsId = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    QuestionnaireId = table.Column<Guid>(nullable: false),
                    Nameoforganisation = table.Column<string>(nullable: true),
                    RegisteredAddress = table.Column<string>(nullable: true),
                    CompanyCharityNumber = table.Column<string>(nullable: true),
                    Sector = table.Column<string>(nullable: true),
                    Turnover = table.Column<string>(nullable: true),
                    NumberofEmployees = table.Column<string>(nullable: true),
                    Nameofmaincontact = table.Column<string>(nullable: true),
                    ContactJobtitle = table.Column<string>(nullable: true),
                    Dateofresponse = table.Column<string>(nullable: true),
                    ContactEmail = table.Column<string>(nullable: true),
                    ContactTelephone = table.Column<string>(nullable: true),
                    CELevelDesired = table.Column<string>(nullable: true),
                    CanCRESTpubliciseyoursuccessfulcertification = table.Column<string>(nullable: true),
                    Cost = table.Column<int>(nullable: false),
                    Days = table.Column<int>(nullable: false),
                    Requirement = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CEQOrgDetails", x => x.CEQOrgDetailsId);
                });

            migrationBuilder.CreateTable(
                name: "CEQQuestionResponse",
                columns: table => new
                {
                    CEQResponseId = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    OptionType = table.Column<int>(nullable: false),
                    OptionId = table.Column<Guid>(nullable: false),
                    OptionDate = table.Column<DateTime>(nullable: false),
                    QuestionnaireId = table.Column<Guid>(nullable: false),
                    Cost = table.Column<int>(nullable: false),
                    Days = table.Column<int>(nullable: false),
                    Requirement = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CEQQuestionResponse", x => x.CEQResponseId);
                });

            migrationBuilder.CreateTable(
                name: "CEQQuestions",
                columns: table => new
                {
                    CEQQuestionId = table.Column<Guid>(nullable: false),
                    QuestionNumber = table.Column<string>(nullable: true),
                    QuestionType = table.Column<int>(nullable: false),
                    QuestionText = table.Column<string>(nullable: true),
                    StandardRecommendation = table.Column<string>(nullable: true),
                    QuestionnaireId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    RowNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CEQQuestions", x => x.CEQQuestionId);
                });

            migrationBuilder.CreateTable(
                name: "CEQRemoteVulnScan",
                columns: table => new
                {
                    CEQRemoteVulnScanId = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    QuestionnaireId = table.Column<Guid>(nullable: false),
                    IPAddressv4andv6addresses = table.Column<string>(nullable: true),
                    FullyQualifiedDomainName = table.Column<string>(nullable: true),
                    NatureandDescriptionofSystem = table.Column<string>(nullable: true),
                    SystemOwnershipandHosting = table.Column<string>(nullable: true),
                    Ifoutofscopepleaseciteareasonwhy = table.Column<string>(nullable: true),
                    Cost = table.Column<int>(nullable: false),
                    Days = table.Column<int>(nullable: false),
                    Requirement = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CEQRemoteVulnScan", x => x.CEQRemoteVulnScanId);
                });

            migrationBuilder.CreateTable(
                name: "CEQWorkstationAssessment",
                columns: table => new
                {
                    CEQWorkstationAssessmentId = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    QuestionnaireId = table.Column<Guid>(nullable: false),
                    Descriptionofthedevice = table.Column<string>(nullable: true),
                    OperatingSystem = table.Column<string>(nullable: true),
                    Usernameandpassword = table.Column<string>(nullable: true),
                    Confirmationthatthedevice = table.Column<string>(nullable: true),
                    Confirmationthatadminaccess = table.Column<string>(nullable: true),
                    TestLocation = table.Column<string>(nullable: true),
                    Cost = table.Column<int>(nullable: false),
                    Days = table.Column<int>(nullable: false),
                    Requirement = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CEQWorkstationAssessment", x => x.CEQWorkstationAssessmentId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CEQCloudSharedServicesAsses");

            migrationBuilder.DropTable(
                name: "CEQOptions");

            migrationBuilder.DropTable(
                name: "CEQOrgDetails");

            migrationBuilder.DropTable(
                name: "CEQQuestionResponse");

            migrationBuilder.DropTable(
                name: "CEQQuestions");

            migrationBuilder.DropTable(
                name: "CEQRemoteVulnScan");

            migrationBuilder.DropTable(
                name: "CEQWorkstationAssessment");
        }
    }
}
