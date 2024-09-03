﻿// <auto-generated />
using System;
using System.Collections.Generic;
using AnimalAllies.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnimalAllies.Infrastructure.Migrations
{
    [DbContext(typeof(AnimalAlliesDbContext))]
    partial class AnimalAlliesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AnimalAllies.Domain.Models.Species.Breed.Breed", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("SpeciesId")
                        .HasColumnType("uuid");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.ComplexProperty<Dictionary<string, object>>("Name", "AnimalAllies.Domain.Models.Species.Breed.Breed.Name#Name", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");
                        });

                    b.HasKey("Id");

                    b.HasIndex("SpeciesId");

                    b.ToTable("breeds", (string)null);
                });

            modelBuilder.Entity("AnimalAllies.Domain.Models.Species.Species", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.ComplexProperty<Dictionary<string, object>>("Name", "AnimalAllies.Domain.Models.Species.Species.Name#Name", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");
                        });

                    b.HasKey("Id");

                    b.ToTable("species", (string)null);
                });

            modelBuilder.Entity("AnimalAllies.Domain.Models.Volunteer.Pet.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Requisites")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("requisites");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<Guid?>("volunteer_id")
                        .HasColumnType("uuid");

                    b.ComplexProperty<Dictionary<string, object>>("Address", "AnimalAllies.Domain.Models.Volunteer.Pet.Pet.Address#Address", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("city");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("state");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("street");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("zip_code");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("AnimalType", "AnimalAllies.Domain.Models.Volunteer.Pet.Pet.AnimalType#AnimalType", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("BreedId")
                                .HasColumnType("uuid")
                                .HasColumnName("breed_id");

                            b1.Property<Guid>("SpeciesId")
                                .HasColumnType("uuid")
                                .HasColumnName("species_id");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("HelpStatus", "AnimalAllies.Domain.Models.Volunteer.Pet.Pet.HelpStatus#HelpStatus", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("help_status");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Name", "AnimalAllies.Domain.Models.Volunteer.Pet.Pet.Name#Name", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("name");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PetDetails", "AnimalAllies.Domain.Models.Volunteer.Pet.Pet.PetDetails#PetDetails", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<DateOnly>("BirthDate")
                                .HasColumnType("date")
                                .HasColumnName("birth_date");

                            b1.Property<DateTime>("CreationTime")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("creation_time");

                            b1.Property<string>("Description")
                                .IsRequired()
                                .HasMaxLength(1500)
                                .HasColumnType("character varying(1500)");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PetPhysicCharacteristics", "AnimalAllies.Domain.Models.Volunteer.Pet.Pet.PetPhysicCharacteristics#PetPhysicCharacteristics", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Color")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("color");

                            b1.Property<string>("HealthInformation")
                                .IsRequired()
                                .HasMaxLength(2000)
                                .HasColumnType("character varying(2000)")
                                .HasColumnName("health_information");

                            b1.Property<double>("Height")
                                .HasColumnType("double precision")
                                .HasColumnName("height");

                            b1.Property<bool>("IsCastrated")
                                .HasColumnType("boolean")
                                .HasColumnName("is_castrated");

                            b1.Property<bool>("IsVaccinated")
                                .HasColumnType("boolean")
                                .HasColumnName("is_vaccinated");

                            b1.Property<double>("Weight")
                                .HasColumnType("double precision")
                                .HasColumnName("weight");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PhoneNumber", "AnimalAllies.Domain.Models.Volunteer.Pet.Pet.PhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(14)
                                .HasColumnType("character varying(14)")
                                .HasColumnName("phone_number");
                        });

                    b.HasKey("Id");

                    b.HasIndex("volunteer_id");

                    b.ToTable("pets", (string)null);
                });

            modelBuilder.Entity("AnimalAllies.Domain.Models.Volunteer.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Requisites")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("requisites");

                    b.Property<string>("SocialNetworks")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("social_networks");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.ComplexProperty<Dictionary<string, object>>("Description", "AnimalAllies.Domain.Models.Volunteer.Volunteer.Description#VolunteerDescription", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(1500)
                                .HasColumnType("character varying(1500)");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Email", "AnimalAllies.Domain.Models.Volunteer.Volunteer.Email#Email", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("email");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("FullName", "AnimalAllies.Domain.Models.Volunteer.Volunteer.FullName#FullName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("first_name");

                            b1.Property<string>("Patronymic")
                                .HasColumnType("text")
                                .HasColumnName("patronymic");

                            b1.Property<string>("SecondName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("second_name");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Phone", "AnimalAllies.Domain.Models.Volunteer.Volunteer.Phone#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(14)
                                .HasColumnType("character varying(14)")
                                .HasColumnName("phone_number");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("WorkExperience", "AnimalAllies.Domain.Models.Volunteer.Volunteer.WorkExperience#WorkExperience", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("work_experience");
                        });

                    b.HasKey("Id");

                    b.ToTable("volunteers", (string)null);
                });

            modelBuilder.Entity("AnimalAllies.Domain.Models.Species.Breed.Breed", b =>
                {
                    b.HasOne("AnimalAllies.Domain.Models.Species.Species", null)
                        .WithMany("Breeds")
                        .HasForeignKey("SpeciesId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AnimalAllies.Domain.Models.Volunteer.Pet.Pet", b =>
                {
                    b.HasOne("AnimalAllies.Domain.Models.Volunteer.Volunteer", null)
                        .WithMany("Pets")
                        .HasForeignKey("volunteer_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("AnimalAllies.Domain.Models.Volunteer.Pet.PetPhotoDetails", "PetPhotoDetails", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid");

                            b1.HasKey("PetId");

                            b1.ToTable("pets");

                            b1.ToJson("pet_photo_details");

                            b1.WithOwner()
                                .HasForeignKey("PetId");

                            b1.OwnsMany("AnimalAllies.Domain.Models.Volunteer.Pet.PetPhoto", "PetPhotos", b2 =>
                                {
                                    b2.Property<Guid>("PetPhotoDetailsPetId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<bool>("IsMain")
                                        .HasColumnType("boolean")
                                        .HasColumnName("is_main");

                                    b2.Property<string>("Path")
                                        .IsRequired()
                                        .HasMaxLength(260)
                                        .HasColumnType("character varying(260)")
                                        .HasColumnName("path");

                                    b2.HasKey("PetPhotoDetailsPetId", "Id");

                                    b2.ToTable("pets");

                                    b2.WithOwner()
                                        .HasForeignKey("PetPhotoDetailsPetId");
                                });

                            b1.Navigation("PetPhotos");
                        });

                    b.Navigation("PetPhotoDetails")
                        .IsRequired();
                });

            modelBuilder.Entity("AnimalAllies.Domain.Models.Species.Species", b =>
                {
                    b.Navigation("Breeds");
                });

            modelBuilder.Entity("AnimalAllies.Domain.Models.Volunteer.Volunteer", b =>
                {
                    b.Navigation("Pets");
                });
#pragma warning restore 612, 618
        }
    }
}
