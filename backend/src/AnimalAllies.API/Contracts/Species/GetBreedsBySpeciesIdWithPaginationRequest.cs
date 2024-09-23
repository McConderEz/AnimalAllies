using AnimalAllies.Application.Features.Species.Queries.GetBreedsBySpeciesId;

namespace AnimalAllies.API.Contracts.Volunteer;

public record GetBreedsBySpeciesIdWithPaginationRequest(
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetBreedsBySpeciesIdWithPaginationQuery ToQuery(Guid speciesId)
        => new(
            speciesId,
            SortBy,
            SortDirection,
            Page,
            PageSize);
}