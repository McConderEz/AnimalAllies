using System.Data;
using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimalAllies.Infrastructure.Configurations;

public class VolunteerConfiguration: IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Phone).IsUnique();
        
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("ck_volunteer_workExp", "WorkExperience >= 0");
            t.HasCheckConstraint("ck_volunteer_petsNeedsHelp", "PetsNeedsHelp >= 0");
            t.HasCheckConstraint("ck_volunteer_petsSearchingHome", "PetsSearchingHome >= 0");
            t.HasCheckConstraint("ck_volunteer_petsFoundHome", "PetsFoundHome >= 0");
        });
        
        builder.Property(x => x.Description)
            .HasMaxLength(Constraints.MAX_DESCRIPTION_LENGTH)
            .IsRequired();

        builder.Property(x => x.WorkExperience)
            .IsRequired();

        builder.Property(x => x.PetsNeedsHelp)
            .IsRequired();

        builder.Property(x => x.PetsSearchingHome)
            .IsRequired();

        builder.Property(x => x.PetsFoundHome)
            .IsRequired();

        builder.ComplexProperty(x => x.FullName, f =>
        {
            f.IsRequired();
            f.Property(x => x.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(50);
            f.Property(x => x.SecondName)
                .HasColumnName("second_name")
                .HasMaxLength(50);
            f.Property(x => x.Patronymic)
                .HasColumnName("patronymic")
                .HasMaxLength(50);
        });

        builder.OwnsMany(x => x.SocialNetworks, b => b.ToJson());
        builder.OwnsMany(x => x.Requisites, b => b.ToJson());
        
        builder.HasMany(x => x.Pets)
            .WithOne()
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}