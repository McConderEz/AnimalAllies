using AnimalAllies.Core.DTOs;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.Volunteer.Application.VolunteerManagement.Queries.GetPetsByBreedId;
using AnimalAllies.Volunteer.Application.VolunteerManagement.Queries.GetPetsBySpeciesId;
using AnimalAllies.Volunteer.Contracts;

namespace AnimalAllies.Volunteer.Presentation;

public class VolunteerContract: IVolunteerContract
{
    private readonly GetPetsBySpeciesIdHandler _getPetsBySpeciesIdHandler;
    private readonly GetPetsByBreedIdHandler _getPetsByBreedIdHandler;

    public VolunteerContract(
        GetPetsBySpeciesIdHandler getPetsBySpeciesIdHandler,
        GetPetsByBreedIdHandler getPetsByBreedIdHandler)
    {
        _getPetsBySpeciesIdHandler = getPetsBySpeciesIdHandler;
        _getPetsByBreedIdHandler = getPetsByBreedIdHandler;
    }

    public async Task<Result<List<PetDto>>> GetPetsBySpeciesId(
        Guid speciesId, 
        CancellationToken cancellationToken = default)
    {
        var query = new GetPetsBySpeciesIdQuery(speciesId);

        return await _getPetsBySpeciesIdHandler.Handle(query, cancellationToken);
    }

    public async Task<Result<List<PetDto>>> GetPetsByBreedId(
        Guid breedId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPetsByBreedIdQuery(breedId);
        
        return await _getPetsByBreedIdHandler.Handle(query, cancellationToken);
    }
}