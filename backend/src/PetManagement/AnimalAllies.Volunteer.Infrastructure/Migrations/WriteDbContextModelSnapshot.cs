﻿// <auto-generated />
using System;
using System.Collections.Generic;
using AnimalAllies.Volunteer.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AnimalAllies.Volunteer.Infrastructure.Migrations
{
    [DbContext(typeof(WriteDbContext))]
    partial class WriteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

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

                    b.ComplexProperty<Dictionary<string, object>>("Description", "AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer.Description#VolunteerDescription", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(1500)
                                .HasColumnType("character varying(1500)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Email", "AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer.Email#Email", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("email");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("FullName", "AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer.FullName#FullName", b1 =>
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

                    b.ComplexProperty<Dictionary<string, object>>("Phone", "AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer.Phone#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(14)
                                .HasColumnType("character varying(14)")
                                .HasColumnName("phone_number");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("WorkExperience", "AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer.WorkExperience#WorkExperience", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("work_experience");
                        });

                    b.HasKey("Id")
                        .HasName("pk_volunteers");

                    b.ToTable("volunteers", (string)null);
                });

            modelBuilder.Entity("AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("PetPhotoDetails")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("pet_photos");

                    b.Property<string>("Requisites")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("requisites");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<Guid?>("volunteer_id")
                        .HasColumnType("uuid")
                        .HasColumnName("volunteer_id");

                    b.ComplexProperty<Dictionary<string, object>>("Address", "AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.Pet.Address#Address", b1 =>
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

                    b.ComplexProperty<Dictionary<string, object>>("AnimalType", "AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.Pet.AnimalType#AnimalType", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("BreedId")
                                .HasColumnType("uuid")
                                .HasColumnName("breed_id");

                            b1.Property<Guid>("SpeciesId")
                                .HasColumnType("uuid")
                                .HasColumnName("species_id");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("HelpStatus", "AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.Pet.HelpStatus#HelpStatus", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("help_status");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Name", "AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.Pet.Name#Name", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("name");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PetDetails", "AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.Pet.PetDetails#PetDetails", b1 =>
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
                                .HasColumnType("character varying(1500)")
                                .HasColumnName("pet_details_description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PetPhysicCharacteristics", "AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.Pet.PetPhysicCharacteristics#PetPhysicCharacteristics", b1 =>
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

                    b.ComplexProperty<Dictionary<string, object>>("PhoneNumber", "AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.Pet.PhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(14)
                                .HasColumnType("character varying(14)")
                                .HasColumnName("phone_number");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Position", "AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.Pet.Position#Position", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("position");
                        });

                    b.HasKey("Id")
                        .HasName("pk_pets");

                    b.HasIndex("volunteer_id")
                        .HasDatabaseName("ix_pets_volunteer_id");

                    b.ToTable("pets", (string)null);
                });

            modelBuilder.Entity("AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.Pet", b =>
                {
                    b.HasOne("AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer", null)
                        .WithMany("Pets")
                        .HasForeignKey("volunteer_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_pets_volunteers_volunteer_id");
                });

            modelBuilder.Entity("AnimalAllies.Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer", b =>
                {
                    b.Navigation("Pets");
                });
#pragma warning restore 612, 618
        }
    }
}
