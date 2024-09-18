using AnimalAllies.Application.Features.Volunteer.Queries.GetVolunteersWithPagination;

namespace AnimalAllies.API.Contracts.Volunteer;

public record GetVolunteersWithPaginationRequest(int Page, int PageSize)
{
    public GetVolunteersWithPaginationQuery ToQuery()
        => new(Page, PageSize);
}