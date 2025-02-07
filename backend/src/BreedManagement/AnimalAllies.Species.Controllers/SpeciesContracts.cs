using AnimalAllies.Core.DTOs;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.Species.Application.SpeciesManagement.Queries.GetBreedsBySpeciesId;
using AnimalAllies.Species.Application.SpeciesManagement.Queries.GetSpeciesWithPagination;
using AnimalAllies.Species.Contracts;

namespace AnimalAllies.Species.Presentation;

public class SpeciesContracts: ISpeciesContracts
{
    private readonly GetSpeciesWithPaginationHandlerDapper _getSpeciesWithPaginationHandlerDapper;
    private readonly GetBreedsBySpeciesIdWithPaginationHandlerDapper _getBreedsBySpeciesIdWithPaginationHandlerDapper;

    public SpeciesContracts(
        GetSpeciesWithPaginationHandlerDapper getSpeciesWithPaginationHandlerDapper,
        GetBreedsBySpeciesIdWithPaginationHandlerDapper getBreedsBySpeciesIdWithPaginationHandlerDapper)
    {
        _getSpeciesWithPaginationHandlerDapper = getSpeciesWithPaginationHandlerDapper;
        _getBreedsBySpeciesIdWithPaginationHandlerDapper = getBreedsBySpeciesIdWithPaginationHandlerDapper;
    }

    public async Task<Result<List<SpeciesDto>>> GetSpecies(CancellationToken cancellationToken = default)
    {
        var species = await _getSpeciesWithPaginationHandlerDapper.Handle(cancellationToken);
        if (species.IsFailure)
            return species.Errors;

        return species.Value;
    }

    public async Task<Result<List<BreedDto>>> GetBreedsBySpeciesId(
        Guid speciesId,
        CancellationToken cancellationToken = default)
    {
        var breeds =
            await _getBreedsBySpeciesIdWithPaginationHandlerDapper.Handle(speciesId, cancellationToken);
        if (breeds.IsFailure)
            return breeds.Errors;
        
        return breeds.Value;
    }
}