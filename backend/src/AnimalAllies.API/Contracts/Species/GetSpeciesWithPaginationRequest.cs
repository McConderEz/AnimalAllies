using AnimalAllies.Application.Features.Species.Queries.GetSpeciesWithPagination;
using AnimalAllies.Application.Features.Volunteer.Queries.GetVolunteersWithPagination;

namespace AnimalAllies.API.Contracts.Volunteer;

public record GetSpeciesWithPaginationRequest(
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetSpeciesWithPaginationQuery ToQuery()
        => new(SortBy, SortDirection, Page, PageSize);
}