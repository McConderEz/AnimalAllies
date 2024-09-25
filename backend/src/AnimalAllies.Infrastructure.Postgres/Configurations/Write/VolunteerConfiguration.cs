using System.Text.Json;
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using AnimalAllies.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimalAllies.Infrastructure.Configurations.Write;

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
                .HasMaxLength(Constraints.MAX_DESCRIPTION_LENGTH)
                .HasColumnName("description");
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

        builder.Property(v => v.Requisites)
            .ValueObjectJsonConverter(
                r => new RequisiteDto() { Title = r.Title, Description = r.Description },
                dto => Requisite.Create(dto.Title, dto.Description).Value)
            .HasColumnName("requisites");
        
        builder.Property(v => v.SocialNetworks)
            .ValueObjectJsonConverter(
                sn => new SocialNetworkDto { Title = sn.Title, Url = sn.Url },
                dto => SocialNetwork.Create(dto.Title, dto.Url).Value)
            .HasColumnName("social_networks");
        
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
        
        builder.HasMany(x => x.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}