using System.Text.Json;
using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Domain.Models.Volunteer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimalAllies.Infrastructure.Configurations.Read;

public class VolunteerDtoConfiguration: IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.ToTable("volunteers");
        
        builder.HasKey(x => x.Id);

        builder.HasMany<PetDto>()
            .WithOne()
            .HasForeignKey(p => p.VolunteerId);

        builder.Property(v => v.SocialNetworks)
            .HasConversion(
                sn => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<SocialNetworkDto[]>(json, JsonSerializerOptions.Default)!);

        builder.Property(v => v.Requisites)
            .HasConversion(
                r => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<RequisiteDto[]>(json, JsonSerializerOptions.Default)!);
    }
}