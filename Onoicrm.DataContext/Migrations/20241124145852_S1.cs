using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Onoicrm.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class S1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_BookingGroup_BookingGroupId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_BookingGroup_BookingGroupId",
                table: "Payment");

            migrationBuilder.DropTable(
                name: "BookingGroup");

            migrationBuilder.DropIndex(
                name: "IX_Payment_BookingGroupId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_BookingGroupId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "BookingGroupId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "BookingGroupId",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BookingGroupId",
                table: "Payment",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BookingGroupId",
                table: "Bookings",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookingGroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClinicId = table.Column<long>(type: "bigint", nullable: true),
                    PatientId = table.Column<long>(type: "bigint", nullable: true),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastView = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Payment_BookingGroupId",
                table: "Payment",
                column: "BookingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BookingGroupId",
                table: "Bookings",
                column: "BookingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingGroup_ClinicId",
                table: "BookingGroup",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingGroup_PatientId",
                table: "BookingGroup",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_BookingGroup_BookingGroupId",
                table: "Bookings",
                column: "BookingGroupId",
                principalTable: "BookingGroup",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_BookingGroup_BookingGroupId",
                table: "Payment",
                column: "BookingGroupId",
                principalTable: "BookingGroup",
                principalColumn: "Id");
        }
    }
}
