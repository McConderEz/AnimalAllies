using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimalAllies.Infrastructure.Configurations;

public class PetPhotoConfiguration: IEntityTypeConfiguration<PetPhoto>
{
    public void Configure(EntityTypeBuilder<PetPhoto> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                Id => PetPhotoId.Create(Id));

        builder.Property(x => x.Path)
            .HasMaxLength(Constraints.MAX_PATH_LENGHT)
            .IsRequired();
    }
}