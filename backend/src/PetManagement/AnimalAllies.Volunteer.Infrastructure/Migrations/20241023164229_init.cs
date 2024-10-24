using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimalAllies.Volunteer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Phone",
                table: "volunteers");

            migrationBuilder.DropIndex(
                name: "IX_Address",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "social_networks",
                table: "volunteers");

            migrationBuilder.EnsureSchema(
                name: "volunteers");

            migrationBuilder.RenameTable(
                name: "volunteers",
                newName: "volunteers",
                newSchema: "volunteers");

            migrationBuilder.RenameTable(
                name: "pets",
                newName: "pets",
                newSchema: "volunteers");

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                schema: "volunteers",
                table: "volunteers",
                type: "character varying(14)",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "deletion_date",
                schema: "volunteers",
                table: "volunteers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "deletion_date",
                schema: "volunteers",
                table: "pets",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deletion_date",
                schema: "volunteers",
                table: "volunteers");

            migrationBuilder.DropColumn(
                name: "deletion_date",
                schema: "volunteers",
                table: "pets");

            migrationBuilder.RenameTable(
                name: "volunteers",
                schema: "volunteers",
                newName: "volunteers");

            migrationBuilder.RenameTable(
                name: "pets",
                schema: "volunteers",
                newName: "pets");

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                table: "volunteers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(14)",
                oldMaxLength: 14);

            migrationBuilder.AddColumn<string>(
                name: "social_networks",
                table: "volunteers",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Phone",
                table: "volunteers",
                column: "phone_number");

            migrationBuilder.CreateIndex(
                name: "IX_Address",
                table: "pets",
                columns: new[] { "city", "state", "zip_code", "street" });
        }
    }
}
