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
        builder.ToTable("volunteers");
        builder.HasKey(x => x.Id);
        
        
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                id => VolunteerId.Create(id));
        
        builder
            .ToTable(t =>
        {
            t.HasCheckConstraint("CK_Volunteer_WorkExperience", "\"WorkExperience\" >= 0");
        });
        
        builder.ComplexProperty(x => x.Description, b =>
        {
            b.IsRequired();
            b.Property(x => x.Value)
                .HasMaxLength(Constraints.MAX_DESCRIPTION_LENGTH);
        });

        builder.Property(x => x.WorkExperience)
            .IsRequired();
        
        builder.ComplexProperty(x => x.Phone, p =>
        {
            p.IsRequired();
            p.Property(x => x.Number)
                .HasColumnName("phone_number")
                .HasMaxLength(14);
        });
        
        builder.ComplexProperty(x => x.Email, p =>
        {
            p.IsRequired();
            p.Property(x => x.Value)
                .HasColumnName("email");
        });

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

        builder.OwnsOne(v => v.Details, d =>
        {
            d.ToJson();

            d.OwnsMany(d => d.SocialNetworks, s =>
            {
                s.Property(sn => sn.Url)
                    .IsRequired()
                    .HasMaxLength(Constraints.MAX_URL_LENGTH);
                s.Property(sn => sn.Title)
                    .IsRequired()
                    .HasMaxLength(Constraints.MAX_VALUE_LENGTH);
            });
            
            d.OwnsMany(d => d.Requisites, r =>
            {
                r.Property(r => r.Description)
                    .IsRequired()
                    .HasMaxLength(Constraints.MAX_DESCRIPTION_LENGTH);
                r.Property(r => r.Title)
                    .IsRequired()
                    .HasMaxLength(Constraints.MAX_VALUE_LENGTH);
            });
            
        });
        
        builder.HasMany(x => x.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}