using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Models.Species.Breed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimalAllies.Infrastructure.Configurations.Write;

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
                .HasMaxLength(Constraints.MAX_VALUE_LENGTH)
                .HasColumnName("name");
        });

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}