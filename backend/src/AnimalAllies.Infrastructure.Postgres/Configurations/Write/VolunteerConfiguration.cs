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
        
        
        builder.Property(v => v.SocialNetworks)
            .HasConversion(
                sn => JsonSerializer.Serialize(sn, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<SocialNetworkDto>>(json, JsonSerializerOptions.Default)!
                    .Select(dto => SocialNetwork.Create(dto.Title,dto.Url).Value)
                    .ToList(),
                new ValueComparer<IReadOnlyList<SocialNetwork>>(
                    (c1,c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0,(a,v) => HashCode.Combine(a,v.GetHashCode())),
                    c => c.ToList()))
            .HasColumnType("jsonb");
        
        builder.Property(v => v.Requisites)
            .HasConversion(
                r => JsonSerializer.Serialize(r, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<RequisiteDto>>(json, JsonSerializerOptions.Default)!
                    .Select(dto => Requisite.Create(dto.Title,dto.Description).Value)
                    .ToList(),
                new ValueComparer<IReadOnlyList<Requisite>>(
                    (c1,c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0,(a,v) => HashCode.Combine(a,v.GetHashCode())),
                    c => c.ToList()))
            .HasColumnType("jsonb");
        
        /*builder.OwnsOne(v => v.SocialNetworks, sb =>
        {
            sb.ToJson("social_networks");
            sb.OwnsMany(sb => sb.Values, sb =>
            {
                sb.Property(s => s.Title)
                    .HasMaxLength(Constraints.MAX_VALUE_LENGTH)
                    .HasColumnName("title")
                    .IsRequired();

                sb.Property(s => s.Url)
                    .HasMaxLength(Constraints.MAX_URL_LENGTH)
                    .HasColumnName("url")
                    .IsRequired();
            });
        });
        
        builder.OwnsOne(v => v.Requisites, rb =>
        {
            rb.ToJson("requisites");
            rb.OwnsMany(rb => rb.Values, rb =>
            {
                rb.Property(r => r.Title)
                    .HasMaxLength(Constraints.MAX_VALUE_LENGTH)
                    .HasColumnName("title")
                    .IsRequired();

                rb.Property(r => r.Description)
                    .HasMaxLength(Constraints.MAX_DESCRIPTION_LENGTH)
                    .HasColumnName("description")
                    .IsRequired();
            });
        });*/
        
        /*builder.Property(v => v.Requisites)
            .HasValueJsonConverter()
            .HasColumnType("jsonb")
            .HasColumnName("requisites");
        
        builder.Property(v => v.SocialNetworks)
            .HasValueJsonConverter()
            .HasColumnType("jsonb")
            .HasColumnName("social_networks");*/
        
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
        
        builder.HasMany(x => x.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}