using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Database;
using AnimalAllies.Application.Extension;
using AnimalAllies.Application.Models;
using AnimalAllies.Domain.Shared;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.Queries.GetVolunteersWithPagination;

public class GetVolunteersWithPaginationHandler :
    IQueryHandler<PagedList<VolunteerDto>, GetFilteredVolunteersWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetFilteredVolunteersWithPaginationQuery> _validator;
    private readonly ILogger<GetVolunteersWithPaginationHandler> _logger;

    public GetVolunteersWithPaginationHandler(
        IReadDbContext readDbContext, 
        IValidator<GetFilteredVolunteersWithPaginationQuery> validator,
        ILogger<GetVolunteersWithPaginationHandler> logger)
    {
        _readDbContext = readDbContext;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<PagedList<VolunteerDto>>> Handle(
        GetFilteredVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            validationResult.ToErrorList();

        var volunteerQuery = _readDbContext.Volunteers;

        volunteerQuery = volunteerQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.FirstName),
            v => v.FirstName.Contains(query.FirstName));
        
        volunteerQuery = volunteerQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.SecondName),
            v => v.SecondName.Contains(query.SecondName));
        
        volunteerQuery = volunteerQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Patronymic),
            v => v.Patronymic.Contains(query.Patronymic));
        
        volunteerQuery = volunteerQuery.WhereIf(
            query.WorkExperienceFrom != null,
            v => v.WorkExperience >= query.WorkExperienceFrom);
        
        volunteerQuery = volunteerQuery.WhereIf(
            query.WorkExperienceTo != null,
            v => v.WorkExperience <= query.WorkExperienceTo);
        
        var pagedList = await volunteerQuery.ToPagedList(
            query.Page,
            query.PageSize,
            cancellationToken);
        
        _logger.LogInformation("Get volunteers");

        return pagedList;
    }
}

public class GetVolunteersWithPaginationHandlerDapper :
    IQueryHandler<PagedList<VolunteerDto>, GetFilteredVolunteersWithPaginationQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<GetVolunteersWithPaginationHandler> _logger;

    public GetVolunteersWithPaginationHandlerDapper(
        ISqlConnectionFactory sqlConnectionFactory,
        ILogger<GetVolunteersWithPaginationHandler> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _logger = logger;
    }

    public async Task<Result<PagedList<VolunteerDto>>> Handle(
        GetFilteredVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();

        var totalCount = await connection.ExecuteScalarAsync<long>("select count(*) from volunteers");
        
        parameters.Add("@PageSize", query.PageSize);
        parameters.Add("@Offset", (query.Page - 1) * query.PageSize);

        //TODO: Не маппит реквизиты и соцсети
        
        var sql = """
                  select 
                      id,
                      first_name,
                      second_name,
                      patronymic,
                      description,
                      email,
                      phone_number,
                      work_experience,
                      from volunteers
                  order by id limit @PageSize offset @Offset
                  """;

        var volunteers = await connection.QueryAsync<VolunteerDto>(sql, parameters);
        
        _logger.LogInformation("Get volunteers");

        return new PagedList<VolunteerDto>
        {
            Items = volunteers.ToList(),
            PageSize = query.PageSize,
            Page = query.Page,
            TotalCount = totalCount
        };
    }
}
