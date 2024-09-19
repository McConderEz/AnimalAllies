using AnimalAllies.Application.Contracts.DTOs;
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

    }
}