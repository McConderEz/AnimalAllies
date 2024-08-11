using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimalAllies.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpeciesList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Volunteers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false),
                    WorkExperience = table.Column<int>(type: "integer", nullable: false),
                    PetsNeedsHelp = table.Column<int>(type: "integer", nullable: false),
                    PetsSearchingHome = table.Column<int>(type: "integer", nullable: false),
                    PetsFoundHome = table.Column<int>(type: "integer", nullable: false),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    patronymic = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    second_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    Requisites = table.Column<string>(type: "jsonb", nullable: true),
                    SocialNetworks = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteers", x => x.Id);
                    table.CheckConstraint("CK_Volunteer_PetsFoundHome", "\"PetsFoundHome\" >= 0");
                    table.CheckConstraint("CK_Volunteer_PetsNeedsHelp", "\"PetsNeedsHelp\" >= 0");
                    table.CheckConstraint("CK_Volunteer_PetsSearchingHome", "\"PetsSearchingHome\" >= 0");
                    table.CheckConstraint("CK_Volunteer_WorkExperience", "\"WorkExperience\" >= 0");
                });

            migrationBuilder.CreateTable(
                name: "Breed",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SpeciesId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Breed_SpeciesList_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "SpeciesList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false),
                    Color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HealthInformation = table.Column<string>(type: "text", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    Height = table.Column<double>(type: "double precision", nullable: false),
                    IsCastrated = table.Column<bool>(type: "boolean", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsVaccinated = table.Column<bool>(type: "boolean", nullable: false),
                    SpeciesID = table.Column<int>(type: "integer", nullable: false),
                    BreedName = table.Column<string>(type: "text", nullable: false),
                    VolunteerId = table.Column<Guid>(type: "uuid", nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    district = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    flat_number = table.Column<int>(type: "integer", maxLength: 20, nullable: false),
                    house_number = table.Column<int>(type: "integer", maxLength: 30, nullable: false),
                    help_status = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    Requisites = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.Id);
                    table.CheckConstraint("CK_Pet_Height", "\"Height\" > 0");
                    table.CheckConstraint("CK_Pet_Weight", "\"Weight\" > 0");
                    table.ForeignKey(
                        name: "FK_Pets_Volunteers_VolunteerId",
                        column: x => x.VolunteerId,
                        principalTable: "Volunteers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PetPhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Path = table.Column<string>(type: "character varying(260)", maxLength: 260, nullable: false),
                    IsMain = table.Column<bool>(type: "boolean", nullable: false),
                    PetId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetPhotos_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Breed_SpeciesId",
                table: "Breed",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_PetPhotos_PetId",
                table: "PetPhotos",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_VolunteerId",
                table: "Pets",
                column: "VolunteerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Breed");

            migrationBuilder.DropTable(
                name: "PetPhotos");

            migrationBuilder.DropTable(
                name: "SpeciesList");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "Volunteers");
        }
    }
}
