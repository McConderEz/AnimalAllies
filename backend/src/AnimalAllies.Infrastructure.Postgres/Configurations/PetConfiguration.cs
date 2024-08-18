using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.ValueObjects;
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

        builder.ComplexProperty(x => x.SpeciesID, b =>
        {
            b.IsRequired();
            b.Property(x => x.Id).HasColumnName("species_id");
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

            b.Property(x => x.Color)
                .HasMaxLength(Constraints.MAX_PET_COLOR_LENGTH);

            b.Property(x => x.Weight)
                .HasColumnName("weight");

            b.Property(x => x.Height)
                .HasColumnName("height");
        });
            

        builder.ComplexProperty(x => x.Address, a =>
        {
            a.IsRequired();
            a.Property(x => x.City)
                .HasColumnName("city")
                .HasMaxLength(100);
            a.Property(x => x.District)
                .HasColumnName("district")
                .HasMaxLength(100);
            a.Property(x => x.HouseNumber)
                .HasColumnName("house_number")
                .HasMaxLength(30);
            a.Property(x => x.FlatNumber)
                .HasColumnName("flat_number")
                .HasMaxLength(20);
            
        });

        builder.ComplexProperty(x => x.Phone, p =>
        {
            p.IsRequired();
            p.Property(x => x.Number)
                .HasColumnName("phone_number")
                .HasMaxLength(14);
        });

        builder.ComplexProperty(x => x.HelpStatus, h =>
        {
            h.IsRequired();
            h.Property(x => x.Value)
                .HasColumnName("help_status");
        });

        

        builder.HasMany(x => x.PetPhotos)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsMany(x => x.Requisites, b => b.ToJson());

    }
}