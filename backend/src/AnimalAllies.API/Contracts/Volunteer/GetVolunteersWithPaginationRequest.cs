using AnimalAllies.Application.Features.Volunteer.Queries.GetVolunteersWithPagination;

namespace AnimalAllies.API.Contracts.Volunteer;

public record GetVolunteersWithPaginationRequest(
    string? FirstName,
    string? SecondName,
    string? Patronymic,
    int? WorkExperienceFrom,
    int? WorkExperienceTo,
    int Page,
    int PageSize)
{
    public GetFilteredVolunteersWithPaginationQuery ToQuery()
        => new(FirstName,
            SecondName,
            Patronymic,
            WorkExperienceFrom,
            WorkExperienceTo,
            Page,
            PageSize);
}