using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimalAllies.Infrastructure.Configurations;

public class BreedConfiguration: IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                Id => BreedId.Create(Id));

        builder.Property(x => x.Name)
            .HasMaxLength(Constraints.MAX_VALUE_LENGTH)
            .IsRequired();
        
    }
}