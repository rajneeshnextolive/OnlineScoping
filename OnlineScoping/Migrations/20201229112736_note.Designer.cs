﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineScoping.Data;

namespace OnlineScoping.Migrations
{
    [DbContext(typeof(OnlineScopingContext))]
    [Migration("20201229112736_note")]
    partial class note
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("OnlineScoping.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("OnlineScoping.Models.CEQCloudSharedServicesAsses", b =>
                {
                    b.Property<Guid>("CEQCloudSharedServicesAssesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Days")
                        .HasColumnType("int");

                    b.Property<string>("DescriptionOfService")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EvidenceOfcertification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IndependentAuditStandards")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QuestionnaireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Requirement")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Supplier")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CEQCloudSharedServicesAssesId");

                    b.ToTable("CEQCloudSharedServicesAsses");
                });

            modelBuilder.Entity("OnlineScoping.Models.CEQOptions", b =>
                {
                    b.Property<Guid>("CEQOptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OptionText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Response")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("res")
                        .HasColumnType("bit");

                    b.HasKey("CEQOptionId");

                    b.ToTable("CEQOptions");
                });

            modelBuilder.Entity("OnlineScoping.Models.CEQOrgDetails", b =>
                {
                    b.Property<Guid>("CEQOrgDetailsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CELevelDesired")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CanCRESTpubliciseyoursuccessfulcertification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyCharityNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactJobtitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactTelephone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Dateofresponse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Days")
                        .HasColumnType("int");

                    b.Property<string>("Nameofmaincontact")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nameoforganisation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumberofEmployees")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QuestionnaireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RegisteredAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Requirement")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sector")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Turnover")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CEQOrgDetailsId");

                    b.ToTable("CEQOrgDetails");
                });

            modelBuilder.Entity("OnlineScoping.Models.CEQQuestionResponse", b =>
                {
                    b.Property<Guid>("CEQResponseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Days")
                        .HasColumnType("int");

                    b.Property<DateTime>("OptionDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("OptionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("OptionType")
                        .HasColumnType("int");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("QuestionnaireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Requirement")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Response")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Weighting")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("CEQResponseId");

                    b.ToTable("CEQQuestionResponse");
                });

            modelBuilder.Entity("OnlineScoping.Models.CEQQuestions", b =>
                {
                    b.Property<Guid>("CEQQuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("QuestionNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionType")
                        .HasColumnType("int");

                    b.Property<Guid>("QuestionnaireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RowNumber")
                        .HasColumnType("int");

                    b.Property<int>("Section")
                        .HasColumnType("int");

                    b.Property<string>("StandardRecommendation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Weighting")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("CEQQuestionId");

                    b.ToTable("CEQQuestions");
                });

            modelBuilder.Entity("OnlineScoping.Models.CEQRemoteVulnScan", b =>
                {
                    b.Property<Guid>("CEQRemoteVulnScanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Days")
                        .HasColumnType("int");

                    b.Property<string>("FullyQualifiedDomainName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IPAddressv4andv6addresses")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ifoutofscopepleaseciteareasonwhy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NatureandDescriptionofSystem")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QuestionnaireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Requirement")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SystemOwnershipandHosting")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CEQRemoteVulnScanId");

                    b.ToTable("CEQRemoteVulnScan");
                });

            modelBuilder.Entity("OnlineScoping.Models.CEQSignature", b =>
                {
                    b.Property<Guid>("CEQSignatureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Days")
                        .HasColumnType("int");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nameofbusiness")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nameoforganisation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QuestionnaireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Requirement")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SignatureDate")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CEQSignatureId");

                    b.ToTable("CEQSignature");
                });

            modelBuilder.Entity("OnlineScoping.Models.CEQWorkstationAssessment", b =>
                {
                    b.Property<Guid>("CEQWorkstationAssessmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Confirmationthatadminaccess")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Confirmationthatthedevice")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Days")
                        .HasColumnType("int");

                    b.Property<string>("Descriptionofthedevice")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OperatingSystem")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QuestionnaireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Requirement")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TestLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Usernameandpassword")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CEQWorkstationAssessmentId");

                    b.ToTable("CEQWorkstationAssessment");
                });

            modelBuilder.Entity("OnlineScoping.Models.ChildQuestions", b =>
                {
                    b.Property<Guid>("ChildQuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("QuestionNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QuestionnaireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SectionId")
                        .HasColumnType("int");

                    b.Property<int>("questionType")
                        .HasColumnType("int");

                    b.HasKey("ChildQuestionId");

                    b.ToTable("ChildQuestions");
                });

            modelBuilder.Entity("OnlineScoping.Models.Customer", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Client")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEmailSent")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobileNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Questionnaire")
                        .HasColumnType("bit");

                    b.Property<string>("QuestionnaireId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RepliedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("SalesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("SendDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Zip")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerId");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("OnlineScoping.Models.CustomerProposal", b =>
                {
                    b.Property<Guid>("CustomerProposalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DocumentReference")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QuestionnaireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SalesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("CustomerProposalId");

                    b.ToTable("CustomerProposal");
                });

            modelBuilder.Entity("OnlineScoping.Models.CustomerQuestionnair", b =>
                {
                    b.Property<Guid>("CustomerQuestionnairId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsQuestionnaire")
                        .HasColumnType("bit");

                    b.Property<Guid>("QuestionnaireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("CustomerQuestionnairId");

                    b.ToTable("CustomerQuestionnair");
                });

            modelBuilder.Entity("OnlineScoping.Models.CyberEssentialsIndustrySectorList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ItemName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CyberEssentialsIndustrySectorList");
                });

            modelBuilder.Entity("OnlineScoping.Models.CyberEssentialsQuestionnaireSections", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CyberEssentialsQuestionnaireSections");
                });

            modelBuilder.Entity("OnlineScoping.Models.Options", b =>
                {
                    b.Property<Guid>("OptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OptionText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("res")
                        .HasColumnType("bit");

                    b.HasKey("OptionId");

                    b.ToTable("Options");
                });

            modelBuilder.Entity("OnlineScoping.Models.PCIQuestionnaire", b =>
                {
                    b.Property<Guid>("PCIQuestionnaireId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FQDN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ownership")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QuestionnaireId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PCIQuestionnaireId");

                    b.ToTable("PCIQuestionnaire");
                });

            modelBuilder.Entity("OnlineScoping.Models.QuestionResponse", b =>
                {
                    b.Property<Guid>("ResponseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChildQuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Days")
                        .HasColumnType("int");

                    b.Property<bool>("IsChecked")
                        .HasColumnType("bit");

                    b.Property<Guid>("MultipleResponseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("OptionDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("OptionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("OptionType")
                        .HasColumnType("int");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("QuestionnaireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Requirement")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Responsetext")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VariableName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ResponseId");

                    b.ToTable("QuestionResponse");
                });

            modelBuilder.Entity("OnlineScoping.Models.Questionnaire", b =>
                {
                    b.Property<Guid>("QuestionnaireId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QuestionnaireId");

                    b.ToTable("Questionnaire");
                });

            modelBuilder.Entity("OnlineScoping.Models.Questions", b =>
                {
                    b.Property<Guid>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("InputType")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QuestionnaireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RowNumber")
                        .HasColumnType("int");

                    b.Property<int>("SectionId")
                        .HasColumnType("int");

                    b.Property<string>("SubTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("questionType")
                        .HasColumnType("int");

                    b.HasKey("QuestionId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("OnlineScoping.Models.Sales", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEmailSent")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobileNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerId");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("OnlineScoping.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("OnlineScoping.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineScoping.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("OnlineScoping.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
