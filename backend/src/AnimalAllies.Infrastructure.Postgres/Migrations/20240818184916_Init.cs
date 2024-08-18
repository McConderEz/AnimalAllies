using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimalAllies.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "species",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name_Value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_species", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "volunteers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkExperience = table.Column<int>(type: "integer", nullable: false),
                    Description_Value = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    patronymic = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    second_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    Requisites = table.Column<string>(type: "jsonb", nullable: true),
                    SocialNetworks = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_volunteers", x => x.Id);
                    table.CheckConstraint("CK_Volunteer_WorkExperience", "\"WorkExperience\" >= 0");
                });

            migrationBuilder.CreateTable(
                name: "breeds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SpeciesId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name_Value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_breeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_breeds_species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsCastrated = table.Column<bool>(type: "boolean", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsVaccinated = table.Column<bool>(type: "boolean", nullable: false),
                    BreedName = table.Column<string>(type: "text", nullable: false),
                    VolunteerId = table.Column<Guid>(type: "uuid", nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    district = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    flat_number = table.Column<int>(type: "integer", maxLength: 20, nullable: false),
                    house_number = table.Column<int>(type: "integer", maxLength: 30, nullable: false),
                    help_status = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PetDetails_Color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PetDetails_Description = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    height = table.Column<double>(type: "double precision", nullable: false),
                    weight = table.Column<double>(type: "double precision", nullable: false),
                    phone_number = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    species_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Requisites = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pets_volunteers_VolunteerId",
                        column: x => x.VolunteerId,
                        principalTable: "volunteers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "pet_photos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Path = table.Column<string>(type: "character varying(260)", maxLength: 260, nullable: false),
                    IsMain = table.Column<bool>(type: "boolean", nullable: false),
                    PetId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pet_photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pet_photos_pets_PetId",
                        column: x => x.PetId,
                        principalTable: "pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_breeds_SpeciesId",
                table: "breeds",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_pet_photos_PetId",
                table: "pet_photos",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_pets_VolunteerId",
                table: "pets",
                column: "VolunteerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "breeds");

            migrationBuilder.DropTable(
                name: "pet_photos");

            migrationBuilder.DropTable(
                name: "species");

            migrationBuilder.DropTable(
                name: "pets");

            migrationBuilder.DropTable(
                name: "volunteers");
        }
    }
}
