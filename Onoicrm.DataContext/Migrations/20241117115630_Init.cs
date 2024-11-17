using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Onoicrm.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttachedFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StorageId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    Extension = table.Column<string>(type: "text", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachedFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clinics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Caption = table.Column<string>(type: "text", nullable: false),
                    ExpiredDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    GroupUserProfileId = table.Column<long>(type: "bigint", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    MaxDoctorCount = table.Column<int>(type: "integer", nullable: false),
                    MaxChairCount = table.Column<int>(type: "integer", nullable: false),
                    WappiProfileId = table.Column<string>(type: "text", nullable: true),
                    WappiToken = table.Column<string>(type: "text", nullable: true),
                    WorkStartTime = table.Column<string>(type: "text", nullable: true),
                    WorkEndTime = table.Column<string>(type: "text", nullable: true),
                    BookingTimeDuration = table.Column<int>(type: "integer", nullable: false),
                    UseArmchairForBooking = table.Column<bool>(type: "boolean", nullable: false),
                    BookingType = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Caption = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TextColor = table.Column<string>(type: "text", nullable: true),
                    BgColor = table.Column<string>(type: "text", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Groups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InformationSource",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Caption = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformationSource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teeth",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Caption = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    Quarter = table.Column<int>(type: "integer", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teeth", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "СancellationReason",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Caption = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_СancellationReason", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Armchairs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClinicId = table.Column<long>(type: "bigint", nullable: true),
                    Caption = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    WorkTimeFrom = table.Column<string>(type: "text", nullable: false),
                    WorkTimeTo = table.Column<string>(type: "text", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Armchairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Armchairs_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Caption = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    PriceFrom = table.Column<decimal>(type: "numeric", nullable: false),
                    PriceTo = table.Column<decimal>(type: "numeric", nullable: false),
                    LabaratoryCaption = table.Column<string>(type: "text", nullable: true),
                    LabaratoryPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    ClinicId = table.Column<long>(type: "bigint", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Service_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Symptoms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Caption = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CssClass = table.Column<string>(type: "text", nullable: false),
                    ClinicId = table.Column<long>(type: "bigint", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symptoms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Symptoms_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ToothStates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Caption = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CssClass = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    ClinicId = table.Column<long>(type: "bigint", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToothStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToothStates_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceGroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ClinicId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceGroup_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceGroup_Groups_Id",
                        column: x => x.Id,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WhatsAppNumber = table.Column<string>(type: "text", nullable: false),
                    JobPosition = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    GenderId = table.Column<int>(type: "integer", nullable: false),
                    MailingConsent = table.Column<bool>(type: "boolean", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    UserProfileId = table.Column<long>(type: "bigint", nullable: true),
                    ClinicId = table.Column<long>(type: "bigint", nullable: true),
                    InformationSourceId = table.Column<long>(type: "bigint", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patient_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patient_InformationSource_InformationSourceId",
                        column: x => x.InformationSourceId,
                        principalTable: "InformationSource",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Patronymic = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    WhatsAppNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    JobPosition = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    GenderId = table.Column<int>(type: "integer", nullable: false),
                    ClinicId = table.Column<long>(type: "bigint", nullable: true),
                    Salary = table.Column<long>(type: "bigint", nullable: false),
                    SalaryType = table.Column<int>(type: "integer", nullable: false),
                    MailingConsent = table.Column<bool>(type: "boolean", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    InformationSourceId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Data = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    Debit = table.Column<int>(type: "integer", nullable: false),
                    Credit = table.Column<int>(type: "integer", nullable: false),
                    Discount = table.Column<int>(type: "integer", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProfiles_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProfiles_InformationSource_InformationSourceId",
                        column: x => x.InformationSourceId,
                        principalTable: "InformationSource",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Channel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Caption = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ToothId = table.Column<long>(type: "bigint", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Channel_Teeth_ToothId",
                        column: x => x.ToothId,
                        principalTable: "Teeth",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceGroupLink",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceGroupId = table.Column<long>(type: "bigint", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceGroupLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceGroupLink_ServiceGroup_ServiceGroupId",
                        column: x => x.ServiceGroupId,
                        principalTable: "ServiceGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceGroupLink_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingGroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    ClinicId = table.Column<long>(type: "bigint", nullable: true),
                    PatientId = table.Column<long>(type: "bigint", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingGroup_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookingGroup_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PatientSymptom",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClinicId = table.Column<long>(type: "bigint", nullable: true),
                    PatientId = table.Column<long>(type: "bigint", nullable: true),
                    SymptomId = table.Column<long>(type: "bigint", nullable: true),
                    Value = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientSymptom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientSymptom_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientSymptom_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientSymptom_Symptoms_SymptomId",
                        column: x => x.SymptomId,
                        principalTable: "Symptoms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DoctorServiceGroupSalary",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClinicId = table.Column<long>(type: "bigint", nullable: false),
                    Salary = table.Column<double>(type: "double precision", nullable: false),
                    SalaryType = table.Column<int>(type: "integer", nullable: false),
                    DoctorId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceGroupId = table.Column<long>(type: "bigint", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorServiceGroupSalary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorServiceGroupSalary_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorServiceGroupSalary_ServiceGroup_ServiceGroupId",
                        column: x => x.ServiceGroupId,
                        principalTable: "ServiceGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorServiceGroupSalary_UserProfiles_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentPlan",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClinicId = table.Column<long>(type: "bigint", nullable: false),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    DoctorId = table.Column<long>(type: "bigint", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ToothId = table.Column<long>(type: "bigint", nullable: false),
                    Caption = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreatmentPlan_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreatmentPlan_Teeth_ToothId",
                        column: x => x.ToothId,
                        principalTable: "Teeth",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreatmentPlan_UserProfiles_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreatmentPlan_UserProfiles_PatientId",
                        column: x => x.PatientId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Sum = table.Column<int>(type: "integer", nullable: false),
                    Caption = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ProfileId = table.Column<long>(type: "bigint", nullable: true),
                    PatientId = table.Column<long>(type: "bigint", nullable: true),
                    BookingGroupId = table.Column<long>(type: "bigint", nullable: true),
                    Method = table.Column<string>(type: "text", nullable: false),
                    ClinicId = table.Column<long>(type: "bigint", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_BookingGroup_BookingGroupId",
                        column: x => x.BookingGroupId,
                        principalTable: "BookingGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payment_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payment_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payment_UserProfiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Complaint = table.Column<string>(type: "text", nullable: false),
                    Caption = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DateTimeStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTimeEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DoctorId = table.Column<long>(type: "bigint", nullable: false),
                    PatientId = table.Column<long>(type: "bigint", nullable: true),
                    ArmchairId = table.Column<long>(type: "bigint", nullable: true),
                    ClinicId = table.Column<long>(type: "bigint", nullable: false),
                    Salary = table.Column<long>(type: "bigint", nullable: false),
                    SalaryType = table.Column<int>(type: "integer", nullable: false),
                    TreatmentPlanId = table.Column<long>(type: "bigint", nullable: true),
                    Discount = table.Column<long>(type: "bigint", nullable: false),
                    DiscountType = table.Column<int>(type: "integer", nullable: false),
                    BookingGroupId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceGroupId = table.Column<long>(type: "bigint", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Armchairs_ArmchairId",
                        column: x => x.ArmchairId,
                        principalTable: "Armchairs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookings_BookingGroup_BookingGroupId",
                        column: x => x.BookingGroupId,
                        principalTable: "BookingGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookings_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookings_ServiceGroup_ServiceGroupId",
                        column: x => x.ServiceGroupId,
                        principalTable: "ServiceGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookings_TreatmentPlan_TreatmentPlanId",
                        column: x => x.TreatmentPlanId,
                        principalTable: "TreatmentPlan",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookings_UserProfiles_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BookingId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingFiles_AttachedFiles_Id",
                        column: x => x.Id,
                        principalTable: "AttachedFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingFiles_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingTooth",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ToothId = table.Column<long>(type: "bigint", nullable: false),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    ToothStateId = table.Column<long>(type: "bigint", nullable: true),
                    Caption = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingTooth", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingTooth_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingTooth_Teeth_ToothId",
                        column: x => x.ToothId,
                        principalTable: "Teeth",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingTooth_ToothStates_ToothStateId",
                        column: x => x.ToothStateId,
                        principalTable: "ToothStates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookingСancellationReason",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    CancellationReasonId = table.Column<long>(type: "bigint", nullable: false),
                    ClinicId = table.Column<long>(type: "bigint", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingСancellationReason", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingСancellationReason_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingСancellationReason_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingСancellationReason_СancellationReason_CancellationRe~",
                        column: x => x.CancellationReasonId,
                        principalTable: "СancellationReason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookingId = table.Column<long>(type: "bigint", nullable: true),
                    Caption = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookingToothChannel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookingToothId = table.Column<long>(type: "bigint", nullable: false),
                    ChannelId = table.Column<long>(type: "bigint", nullable: false),
                    MasterCon = table.Column<long>(type: "bigint", nullable: false),
                    MasterFile = table.Column<long>(type: "bigint", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingToothChannel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingToothChannel_BookingTooth_BookingToothId",
                        column: x => x.BookingToothId,
                        principalTable: "BookingTooth",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingToothChannel_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingToothFile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BookingToothId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingToothFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingToothFile_AttachedFiles_Id",
                        column: x => x.Id,
                        principalTable: "AttachedFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingToothFile_BookingTooth_BookingToothId",
                        column: x => x.BookingToothId,
                        principalTable: "BookingTooth",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ImplementedService",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    BookingToothId = table.Column<long>(type: "bigint", nullable: true),
                    DoctorId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceGroupId = table.Column<long>(type: "bigint", nullable: true),
                    LabaratoryPrice = table.Column<long>(type: "bigint", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Caption = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Salary = table.Column<long>(type: "bigint", nullable: false),
                    SalaryType = table.Column<int>(type: "integer", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImplementedService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImplementedService_BookingTooth_BookingToothId",
                        column: x => x.BookingToothId,
                        principalTable: "BookingTooth",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ImplementedService_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImplementedService_ServiceGroup_ServiceGroupId",
                        column: x => x.ServiceGroupId,
                        principalTable: "ServiceGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ImplementedService_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImplementedService_UserProfiles_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Armchairs_ClinicId",
                table: "Armchairs",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingFiles_BookingId",
                table: "BookingFiles",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingGroup_ClinicId",
                table: "BookingGroup",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingGroup_PatientId",
                table: "BookingGroup",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ArmchairId",
                table: "Bookings",
                column: "ArmchairId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BookingGroupId",
                table: "Bookings",
                column: "BookingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ClinicId",
                table: "Bookings",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_DoctorId",
                table: "Bookings",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PatientId",
                table: "Bookings",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ServiceGroupId",
                table: "Bookings",
                column: "ServiceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TreatmentPlanId",
                table: "Bookings",
                column: "TreatmentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingTooth_BookingId",
                table: "BookingTooth",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingTooth_ToothId",
                table: "BookingTooth",
                column: "ToothId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingTooth_ToothStateId",
                table: "BookingTooth",
                column: "ToothStateId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingToothChannel_BookingToothId",
                table: "BookingToothChannel",
                column: "BookingToothId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingToothChannel_ChannelId",
                table: "BookingToothChannel",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingToothFile_BookingToothId",
                table: "BookingToothFile",
                column: "BookingToothId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingСancellationReason_BookingId",
                table: "BookingСancellationReason",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingСancellationReason_CancellationReasonId",
                table: "BookingСancellationReason",
                column: "CancellationReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingСancellationReason_ClinicId",
                table: "BookingСancellationReason",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Channel_ToothId",
                table: "Channel",
                column: "ToothId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorServiceGroupSalary_ClinicId",
                table: "DoctorServiceGroupSalary",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorServiceGroupSalary_DoctorId",
                table: "DoctorServiceGroupSalary",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorServiceGroupSalary_ServiceGroupId",
                table: "DoctorServiceGroupSalary",
                column: "ServiceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ParentId",
                table: "Groups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ImplementedService_BookingId",
                table: "ImplementedService",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_ImplementedService_BookingToothId",
                table: "ImplementedService",
                column: "BookingToothId");

            migrationBuilder.CreateIndex(
                name: "IX_ImplementedService_DoctorId",
                table: "ImplementedService",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_ImplementedService_ServiceGroupId",
                table: "ImplementedService",
                column: "ServiceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ImplementedService_ServiceId",
                table: "ImplementedService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_ClinicId",
                table: "Patient",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_InformationSourceId",
                table: "Patient",
                column: "InformationSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientSymptom_ClinicId",
                table: "PatientSymptom",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientSymptom_PatientId",
                table: "PatientSymptom",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientSymptom_SymptomId",
                table: "PatientSymptom",
                column: "SymptomId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_BookingGroupId",
                table: "Payment",
                column: "BookingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ClinicId",
                table: "Payment",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PatientId",
                table: "Payment",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ProfileId",
                table: "Payment",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_BookingId",
                table: "Prices",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_ClinicId",
                table: "Service",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceGroup_ClinicId",
                table: "ServiceGroup",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceGroupLink_ServiceGroupId",
                table: "ServiceGroupLink",
                column: "ServiceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceGroupLink_ServiceId",
                table: "ServiceGroupLink",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Symptoms_ClinicId",
                table: "Symptoms",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_ToothStates_ClinicId",
                table: "ToothStates",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentPlan_ClinicId",
                table: "TreatmentPlan",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentPlan_DoctorId",
                table: "TreatmentPlan",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentPlan_PatientId",
                table: "TreatmentPlan",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentPlan_ToothId",
                table: "TreatmentPlan",
                column: "ToothId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_ClinicId",
                table: "UserProfiles",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_InformationSourceId",
                table: "UserProfiles",
                column: "InformationSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BookingFiles");

            migrationBuilder.DropTable(
                name: "BookingToothChannel");

            migrationBuilder.DropTable(
                name: "BookingToothFile");

            migrationBuilder.DropTable(
                name: "BookingСancellationReason");

            migrationBuilder.DropTable(
                name: "DoctorServiceGroupSalary");

            migrationBuilder.DropTable(
                name: "ImplementedService");

            migrationBuilder.DropTable(
                name: "PatientSymptom");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "ServiceGroupLink");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Channel");

            migrationBuilder.DropTable(
                name: "AttachedFiles");

            migrationBuilder.DropTable(
                name: "СancellationReason");

            migrationBuilder.DropTable(
                name: "BookingTooth");

            migrationBuilder.DropTable(
                name: "Symptoms");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "ToothStates");

            migrationBuilder.DropTable(
                name: "Armchairs");

            migrationBuilder.DropTable(
                name: "BookingGroup");

            migrationBuilder.DropTable(
                name: "ServiceGroup");

            migrationBuilder.DropTable(
                name: "TreatmentPlan");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Teeth");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Clinics");

            migrationBuilder.DropTable(
                name: "InformationSource");
        }
    }
}
