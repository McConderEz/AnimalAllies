using AnimalAllies.Species.Application.SpeciesManagement.Queries.GetBreedsBySpeciesId;

namespace AnimalAllies.Species.Presentation.Requests;

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