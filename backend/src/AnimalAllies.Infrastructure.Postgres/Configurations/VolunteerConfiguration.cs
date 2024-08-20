using System.Data;
using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Volunteer;
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
        
        builder.ComplexProperty(x => x.Description, b =>
        {
            b.IsRequired();
            b.Property(x => x.Value)
                .HasMaxLength(Constraints.MAX_DESCRIPTION_LENGTH);
        });

        builder.ComplexProperty(x => x.WorkExperience, we =>
        {
            we.IsRequired();
            we.Property(x => x.Value)
                .HasColumnName("work_experience");
        });

        builder.ComplexProperty(x => x.Phone, p =>
        {
            p.IsRequired();
            p.Property(x => x.Number)
                .HasColumnName("phone_number")
                .HasMaxLength(Constraints.MAX_PHONENUMBER_LENGTH);
        });
        
        builder.ComplexProperty(x => x.Email, p =>
        {
            p.IsRequired();
            p.Property(x => x.Value)
                .HasColumnName("email");
        });

        builder.ComplexProperty(x => x.FullName, f =>
        {
            f.Property(x => x.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(Constraints.MIDDLE_NAME_LENGTH)
                .IsRequired();
            f.Property(x => x.SecondName)
                .HasColumnName("second_name")
                .HasMaxLength(Constraints.MIDDLE_NAME_LENGTH)
                .IsRequired();
            f.Property(x => x.Patronymic)
                .HasColumnName("patronymic")
                .IsRequired(false);
        });

        builder.OwnsOne(v => v.SocialNetworks, sn =>
        {
            sn.ToJson();

            sn.OwnsMany(d => d.SocialNetworks, s =>
            {
                s.Property(x => x.Url)
                    .IsRequired()
                    .HasMaxLength(Constraints.MAX_URL_LENGTH);
                s.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(Constraints.MAX_VALUE_LENGTH);
            });
            
        });
        
        builder.OwnsOne(v => v.Requisites, r =>
        {
            r.ToJson();

            r.OwnsMany(d => d.Requisites, s =>
            {
                s.Property(sn => sn.Description)
                    .IsRequired()
                    .HasMaxLength(Constraints.MAX_DESCRIPTION_LENGTH);
                s.Property(sn => sn.Title)
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