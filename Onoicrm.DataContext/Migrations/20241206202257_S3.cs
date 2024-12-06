using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Onoicrm.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class S3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BgColor",
                table: "ServiceGroup");

            migrationBuilder.DropColumn(
                name: "Caption",
                table: "ServiceGroup");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "ServiceGroup");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ServiceGroup");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "ServiceGroup");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ServiceGroup");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "ServiceGroup");

            migrationBuilder.DropColumn(
                name: "LastView",
                table: "ServiceGroup");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ServiceGroup");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ServiceGroup");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "ServiceGroup");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "ServiceGroup");

            migrationBuilder.DropColumn(
                name: "TextColor",
                table: "ServiceGroup");

            migrationBuilder.DropColumn(
                name: "UpdateUserId",
                table: "ServiceGroup");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "ServiceGroup",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceGroup_Groups_Id",
                table: "ServiceGroup",
                column: "Id",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceGroup_Groups_Id",
                table: "ServiceGroup");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "ServiceGroup",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "BgColor",
                table: "ServiceGroup",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Caption",
                table: "ServiceGroup",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "ServiceGroup",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "ServiceGroup",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreateUserId",
                table: "ServiceGroup",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ServiceGroup",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "ServiceGroup",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastView",
                table: "ServiceGroup",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ServiceGroup",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "ServiceGroup",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Priority",
                table: "ServiceGroup",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "ServiceGroup",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TextColor",
                table: "ServiceGroup",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdateUserId",
                table: "ServiceGroup",
                type: "bigint",
                nullable: true);
        }
    }
}
