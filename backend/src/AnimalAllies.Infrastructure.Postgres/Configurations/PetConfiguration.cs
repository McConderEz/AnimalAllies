using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimalAllies.Infrastructure.Configurations;

public class PetConfiguration: IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.HasKey(x => x.Id);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("ck_pet_weight", "Weight > 0");
            t.HasCheckConstraint("ck_pet_height", "Height > 0");
            t.HasCheckConstraint("ck_pet_birthdate", "YEAR(BirthDate)<=YEAR(GETDATE())");
        });
        
        builder.HasIndex(x => x.Address);

        builder.Property(x => x.Name)
            .HasMaxLength(Constraints.MAX_VALUE_LENGTH)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(Constraints.MAX_DESCRIPTION_LENGTH)
            .IsRequired();

        builder.Property(x => x.Color)
            .HasMaxLength(Constraints.MAX_PET_COLOR_LENGTH)
            .IsRequired();

        builder.Property(x => x.Weight)
            .IsRequired();

        builder.Property(x => x.Height)
            .IsRequired();

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

        builder.ComplexProperty(x => x.Species, s =>
        {
            s.IsRequired();
            s.Property(x => x.Value)
                .HasColumnName("species");
        });

        builder.ComplexProperty(x => x.AnimalType, a =>
        {
            a.IsRequired();
            a.Property(x => x.Value)
                .HasColumnName("animal_type");
        });

        builder.HasMany(x => x.PetPhotos)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsMany(x => x.Requisites, b => b.ToJson());

    }
}