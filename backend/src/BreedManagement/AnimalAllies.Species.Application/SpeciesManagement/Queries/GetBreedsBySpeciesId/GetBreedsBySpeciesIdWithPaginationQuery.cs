using AnimalAllies.Core.Abstractions;

namespace AnimalAllies.Species.Application.SpeciesManagement.Queries.GetBreedsBySpeciesId;

public record GetBreedsBySpeciesIdWithPaginationQuery(
    Guid SpeciesId,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;