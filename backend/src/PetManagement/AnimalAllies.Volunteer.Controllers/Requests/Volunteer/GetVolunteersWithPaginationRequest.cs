using AnimalAllies.Volunteer.Application.VolunteerManagement.Queries.GetVolunteersWithPagination;

namespace AnimalAllies.Volunteer.Presentation.Requests.Volunteer;

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