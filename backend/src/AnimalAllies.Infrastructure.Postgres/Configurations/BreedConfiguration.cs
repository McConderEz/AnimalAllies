using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Species.Breed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimalAllies.Infrastructure.Configurations;

public class BreedConfiguration: IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                Id => BreedId.Create(Id));

        builder.ComplexProperty(x => x.Name, b =>
        {
            b.IsRequired();

            b.Property(x => x.Value)
                .HasMaxLength(Constraints.MAX_VALUE_LENGTH);
        });

    }
}