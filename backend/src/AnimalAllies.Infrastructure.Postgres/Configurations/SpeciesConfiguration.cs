using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimalAllies.Infrastructure.Configurations;

public class SpeciesConfiguration: IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                Id => SpeciesId.Create(Id));

        builder.ComplexProperty(x => x.Name, b =>
        {
            b.IsRequired();
            b.Property(x => x.Value)
                .HasMaxLength(Constraints.MAX_VALUE_LENGTH);
        });
        
        builder.HasMany(x => x.Breeds)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}