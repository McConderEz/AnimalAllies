using AnimalAllies.Application.Contracts.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AnimalAllies.Application.Database;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }
    IQueryable<BreedDto> Breeds { get; }
    IQueryable<SpeciesDto> Species { get; }
    IQueryable<PetDto> Pets { get; }
}