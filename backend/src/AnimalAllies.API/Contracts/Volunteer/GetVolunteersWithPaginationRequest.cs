using AnimalAllies.Application.Features.Volunteer.Queries.GetVolunteersWithPagination;

namespace AnimalAllies.API.Contracts.Volunteer;

public record GetFilteredVolunteersWithPaginationRequest(
    string? FirstName,
    string? SecondName,
    string? Patronymic,
    int? WorkExperienceFrom,
    int? WorkExperienceTo,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetFilteredVolunteersWithPaginationQuery ToQuery()
        => new(FirstName,
            SecondName,
            Patronymic,
            WorkExperienceFrom,
            WorkExperienceTo,
            SortBy,
            SortDirection,
            Page,
            PageSize);
}