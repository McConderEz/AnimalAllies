using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimalAllies.Species.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "species");

            migrationBuilder.RenameTable(
                name: "species",
                newName: "species",
                newSchema: "species");

            migrationBuilder.RenameTable(
                name: "breeds",
                newName: "breeds",
                newSchema: "species");

            migrationBuilder.AddColumn<DateTime>(
                name: "deletion_date",
                schema: "species",
                table: "species",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "deletion_date",
                schema: "species",
                table: "breeds",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deletion_date",
                schema: "species",
                table: "species");

            migrationBuilder.DropColumn(
                name: "deletion_date",
                schema: "species",
                table: "breeds");

            migrationBuilder.RenameTable(
                name: "species",
                schema: "species",
                newName: "species");

            migrationBuilder.RenameTable(
                name: "breeds",
                schema: "species",
                newName: "breeds");
        }
    }
}
