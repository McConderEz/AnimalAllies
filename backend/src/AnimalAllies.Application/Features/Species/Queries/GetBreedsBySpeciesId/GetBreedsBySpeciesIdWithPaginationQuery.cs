using AnimalAllies.Application.Abstractions;

namespace AnimalAllies.Application.Features.Species.Queries.GetBreedsBySpeciesId;

public record GetBreedsBySpeciesIdWithPaginationQuery(
    Guid SpeciesId,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;