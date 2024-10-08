using AnimalAllies.Core.DTOs;
using AnimalAllies.SharedKernel.Shared;

namespace AnimalAllies.Volunteer.Contracts;

public interface IVolunteerContract
{
    Task<Result<List<PetDto>>> GetPetsBySpeciesId(Guid speciesId, CancellationToken cancellationToken = default);
    Task<Result<List<PetDto>>> GetPetsByBreedId(Guid breedId, CancellationToken cancellationToken = default);
}