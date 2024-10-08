using AnimalAllies.Core.DTOs;
using AnimalAllies.SharedKernel.Shared;

namespace AnimalAllies.Species.Contracts;

public interface ISpeciesContracts
{
    Task<Result<List<SpeciesDto>>> GetSpecies(CancellationToken cancellationToken = default);
    Task<Result<List<BreedDto>>> GetBreedsBySpeciesId(Guid speciesId,CancellationToken cancellationToken = default);
}