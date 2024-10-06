using AnimalAllies.Core.DTOs;

namespace AnimalAllies.Core.Database;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }
    IQueryable<BreedDto> Breeds { get; }
    IQueryable<SpeciesDto> Species { get; }
    IQueryable<PetDto> Pets { get; }
}