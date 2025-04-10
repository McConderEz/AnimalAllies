using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.CheckIfPetByBreedIdExist;
using AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.CheckIfPetBySpeciesIdExist;
using AnimalAllies.Volunteer.Contracts;

namespace AnimalAllies.Volunteer.Presentation;

public class VolunteerContract: IVolunteerContract
{
    private readonly CheckIfPetBySpeciesIdExistHandler _checkIfPetBySpeciesIdExistHandler;
    private readonly CheckIfPetByBreedIdExistHandler _checkIfPetByBreedIdExistHandler;

    public VolunteerContract(
        CheckIfPetBySpeciesIdExistHandler checkIfPetBySpeciesIdExistHandler,
        CheckIfPetByBreedIdExistHandler checkIfPetByBreedIdExistHandler)
    {
        _checkIfPetBySpeciesIdExistHandler = checkIfPetBySpeciesIdExistHandler;
        _checkIfPetByBreedIdExistHandler = checkIfPetByBreedIdExistHandler;
    }
    
    public async Task<bool> CheckIfPetBySpeciesIdExist(
        Guid speciesId,
        CancellationToken cancellationToken = default)
    {
        var query = new CheckIfPetBySpeciesIdExistQuery(speciesId);

        return (await _checkIfPetBySpeciesIdExistHandler.Handle(query, cancellationToken)).Value;
    }

    public async Task<bool> CheckIfPetByBreedIdExist(
        Guid breedId,
        CancellationToken cancellationToken = default)
    {
        var query = new CheckIfPetByBreedIdExistQuery(breedId);

        return (await _checkIfPetByBreedIdExistHandler.Handle(query, cancellationToken)).Value;
    }
}