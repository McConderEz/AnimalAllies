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

            modelBuilder.Entity("AnimalAllies.Domain.Models.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(350)
                        .HasColumnType("character varying(350)");

                    b.Property<double>("Height")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsCastrated")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsVaccinated")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid?>("VolunteerId")
                        .HasColumnType("uuid");

                    b.Property<double>("Weight")
                        .HasColumnType("double precision");

                    b.ComplexProperty<Dictionary<string, object>>("Address", "AnimalAllies.Domain.Models.Pet.Address#Address", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("city");

                            b1.Property<string>("District")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("district");

                            b1.Property<int>("FlatNumber")
                                .HasMaxLength(20)
                                .HasColumnType("integer")
                                .HasColumnName("flat_number");

                            b1.Property<int>("HouseNumber")
                                .HasMaxLength(30)
                                .HasColumnType("integer")
                                .HasColumnName("house_number");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("AnimalType", "AnimalAllies.Domain.Models.Pet.AnimalType#AnimalType", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("animal_type");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("HelpStatus", "AnimalAllies.Domain.Models.Pet.HelpStatus#HelpStatus", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("help_status");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Phone", "AnimalAllies.Domain.Models.Pet.Phone#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(14)
                                .HasColumnType("character varying(14)")
                                .HasColumnName("phone_number");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Species", "AnimalAllies.Domain.Models.Pet.Species#Species", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("species");
                        });

                    b.HasKey("Id");

                    b.HasIndex("VolunteerId");

                    b.ToTable("Pets", t =>
                        {
                            t.HasCheckConstraint("CK_Pet_Height", "\"Height\" > 0");

                            t.HasCheckConstraint("CK_Pet_Weight", "\"Weight\" > 0");
                        });
                });

            modelBuilder.Entity("AnimalAllies.Domain.Models.PetPhoto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(260)
                        .HasColumnType("character varying(260)");

                    b.Property<Guid?>("PetId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PetId");

                    b.ToTable("PetPhotos");
                });

            modelBuilder.Entity("AnimalAllies.Domain.Models.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(350)
                        .HasColumnType("character varying(350)");

                    b.Property<int>("PetsFoundHome")
                        .HasColumnType("integer");

                    b.Property<int>("PetsNeedsHelp")
                        .HasColumnType("integer");

                    b.Property<int>("PetsSearchingHome")
                        .HasColumnType("integer");

                    b.Property<int>("WorkExperience")
                        .HasColumnType("integer");

                    b.ComplexProperty<Dictionary<string, object>>("FullName", "AnimalAllies.Domain.Models.Volunteer.FullName#FullName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("first_name");

                            b1.Property<string>("Patronymic")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("patronymic");

                            b1.Property<string>("SecondName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("second_name");
                        });

                    b.HasKey("Id");

                    b.ToTable("Volunteers", t =>
                        {
                            t.HasCheckConstraint("CK_Volunteer_PetsFoundHome", "\"PetsFoundHome\" >= 0");

                            t.HasCheckConstraint("CK_Volunteer_PetsNeedsHelp", "\"PetsNeedsHelp\" >= 0");

                            t.HasCheckConstraint("CK_Volunteer_PetsSearchingHome", "\"PetsSearchingHome\" >= 0");

                            t.HasCheckConstraint("CK_Volunteer_WorkExperience", "\"WorkExperience\" >= 0");
                        });
                });

            modelBuilder.Entity("AnimalAllies.Domain.Models.Pet", b =>
                {
                    b.HasOne("AnimalAllies.Domain.Models.Volunteer", null)
                        .WithMany("Pets")
                        .HasForeignKey("VolunteerId");

                    b.OwnsMany("AnimalAllies.Domain.ValueObjects.Requisite", "Requisites", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            b1.HasKey("PetId", "Id");

                            b1.ToTable("Pets");

                            b1.ToJson("Requisites");

                            b1.WithOwner()
                                .HasForeignKey("PetId");
                        });

                    b.Navigation("Requisites");
                });

            modelBuilder.Entity("AnimalAllies.Domain.Models.PetPhoto", b =>
                {
                    b.HasOne("AnimalAllies.Domain.Models.Pet", null)
                        .WithMany("PetPhotos")
                        .HasForeignKey("PetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AnimalAllies.Domain.Models.Volunteer", b =>
                {
                    b.OwnsMany("AnimalAllies.Domain.ValueObjects.Requisite", "Requisites", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            b1.HasKey("VolunteerId", "Id");

                            b1.ToTable("Volunteers");

                            b1.ToJson("Requisites");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId");
                        });

                    b.OwnsMany("AnimalAllies.Domain.ValueObjects.SocialNetwork", "SocialNetworks", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            b1.HasKey("VolunteerId", "Id");

                            b1.ToTable("Volunteers");

                            b1.ToJson("SocialNetworks");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId");
                        });

                    b.Navigation("Requisites");

                    b.Navigation("SocialNetworks");
                });

            modelBuilder.Entity("AnimalAllies.Domain.Models.Pet", b =>
                {
                    b.Navigation("PetPhotos");
                });

            modelBuilder.Entity("AnimalAllies.Domain.Models.Volunteer", b =>
                {
                    b.Navigation("Pets");
                });
#pragma warning restore 612, 618
        }
    }
}
