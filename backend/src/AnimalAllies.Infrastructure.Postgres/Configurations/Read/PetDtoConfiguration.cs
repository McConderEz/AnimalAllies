using AnimalAllies.Application.Contracts.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimalAllies.Infrastructure.Configurations.Read;

public class PetDtoConfiguration: IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> builder)
    {
        builder.ToTable("pets");
        builder.HasKey(x => x.Id);
        
        //TODO: Пока без json конвертера
    }
}