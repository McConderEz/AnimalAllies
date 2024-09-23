using AnimalAllies.Application.Abstractions;

namespace AnimalAllies.Application.Features.Species.Queries.GetSpeciesWithPagination;

public record GetSpeciesWithPaginationQuery(
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;