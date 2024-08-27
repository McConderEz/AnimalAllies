using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Species;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimalAllies.Infrastructure.Configurations;

public class PetConfiguration: IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                Id => PetId.Create(Id));

        builder.ComplexProperty(x => x.AnimalType, b =>
        {
            b.IsRequired();
            b.Property(x => x.SpeciesId)
                .HasColumnName("species_id")
                .HasConversion(
                    id => id.Id,
                    id => SpeciesId.Create(id));
            b.Property(x => x.BreedId)
                .HasColumnName("breed_id");
        });

        builder.ComplexProperty(x => x.Name, b =>
        {
            b.IsRequired();
            b.Property(x => x.Value)
                .HasMaxLength(Constraints.MAX_VALUE_LENGTH)
                .HasColumnName("name");
        });

        builder.ComplexProperty(x => x.PetDetails, b =>
        {
            b.IsRequired();
            b.Property(x => x.Description)
                .HasMaxLength(Constraints.MAX_DESCRIPTION_LENGTH);

            b.Property(x => x.BirthDate)
                .HasColumnName("birth_date");

            b.Property(x => x.CreationTime)
                .HasColumnName("creation_time");
        });

        builder.ComplexProperty(x => x.PetPhysicCharacteristics, ppc =>
        {
            ppc.IsRequired();

            ppc.Property(x => x.Color)
                .HasMaxLength(Constraints.MAX_PET_COLOR_LENGTH)
                .HasColumnName("color");
            ppc.Property(x => x.HealthInformation)
                .HasMaxLength(Constraints.MAX_PET_INFORMATION_LENGTH)
                .HasColumnName("health_information");
            ppc.Property(x => x.Height)
                .HasColumnName("height");
            ppc.Property(x => x.Weight)
                .HasColumnName("weight");
            ppc.Property(x => x.IsCastrated)
                .HasColumnName("is_castrated");
            ppc.Property(x => x.IsVaccinated)
                .HasColumnName("is_vaccinated");
        });

        builder.ComplexProperty(x => x.Address, a =>
        {
            a.IsRequired();
            a.Property(x => x.City)
                .HasColumnName("city")
                .HasMaxLength(Constraints.MAX_VALUE_LENGTH);
            a.Property(x => x.Street)
                .HasColumnName("street")
                .HasMaxLength(Constraints.MAX_VALUE_LENGTH);
            a.Property(x => x.State)
                .HasColumnName("state")
                .HasMaxLength(Constraints.MAX_VALUE_LENGTH);
            a.Property(x => x.ZipCode)
                .HasColumnName("zip_code")
                .HasMaxLength(Constraints.MAX_VALUE_LENGTH);
            
        });

        builder.ComplexProperty(x => x.PhoneNumber, p =>
        {
            p.IsRequired();
            p.Property(x => x.Number)
                .HasColumnName("phone_number")
                .HasMaxLength(Constraints.MAX_PHONENUMBER_LENGTH);
        });

        builder.ComplexProperty(x => x.HelpStatus, h =>
        {
            h.IsRequired();
            h.Property(x => x.Value)
                .HasColumnName("help_status");
        });

        builder.OwnsOne(x => x.PetPhotoDetails, ppb =>
        {
            ppb.ToJson("pet_photo_details");

            ppb.OwnsMany(x => x.PetPhotos, ppb =>
            {
                ppb.Property(x => x.Path)
                    .HasMaxLength(Constraints.MAX_PATH_LENGHT)
                    .HasColumnName("path")
                    .IsRequired();
                ppb.Property(x => x.IsMain)
                    .HasColumnName("is_main")
                    .IsRequired();
            });
        });
        
        

        builder.OwnsOne(x => x.Requisites, rb =>
        {
            rb.ToJson("requisites");
            rb.OwnsMany(x => x.Requisites, r =>
            {
                r.Property(x => x.Title)
                    .HasMaxLength(Constraints.MAX_VALUE_LENGTH)
                    .HasColumnName("title")
                    .IsRequired();
                r.Property(x => x.Description)
                    .HasMaxLength(Constraints.MAX_DESCRIPTION_LENGTH)
                    .HasColumnName("description")
                    .IsRequired();
            });
        });

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}