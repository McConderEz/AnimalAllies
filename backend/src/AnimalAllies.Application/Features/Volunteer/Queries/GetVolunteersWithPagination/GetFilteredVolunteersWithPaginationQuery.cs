using AnimalAllies.Application.Abstractions;

namespace AnimalAllies.Application.Features.Volunteer.Queries.GetVolunteersWithPagination;

public record GetFilteredVolunteersWithPaginationQuery(
    string? FirstName,
    string? SecondName,
    string? Patronymic,
    int? WorkExperienceFrom,
    int? WorkExperienceTo,
    int Page,
    int PageSize) : IQuery;
