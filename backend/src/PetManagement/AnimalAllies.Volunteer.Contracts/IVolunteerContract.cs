using AnimalAllies.Core.DTOs;
using AnimalAllies.SharedKernel.Shared;

namespace AnimalAllies.Volunteer.Contracts;

public interface IVolunteerContract
{
    Task<Result<bool>> CheckIfPetBySpeciesIdExist(Guid speciesId, CancellationToken cancellationToken = default);
    Task<Result<bool>> CheckIfPetByBreedIdExist(Guid breedId, CancellationToken cancellationToken = default);
}