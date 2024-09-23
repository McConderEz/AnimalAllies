using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Application.Database;
using AnimalAllies.Application.Extension;
using AnimalAllies.Application.Models;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.Queries.GetVolunteersWithPagination;

public class GetFilteredVolunteersWithPaginationHandler :
    IQueryHandler<PagedList<VolunteerDto>, GetFilteredVolunteersWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IValidator<GetFilteredVolunteersWithPaginationQuery> _validator;
    private readonly ILogger<GetVolunteersWithPaginationHandler> _logger;

    public GetFilteredVolunteersWithPaginationHandler(
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

        var keySelector = SortByProperty(query);

        volunteerQuery = query.SortDirection?.ToLower() == "desc"
                ? volunteerQuery.OrderByDescending(keySelector) 
                : volunteerQuery.OrderBy(keySelector);
        
        volunteerQuery = VolunteerQueryFilter(query, volunteerQuery);
        
        var pagedList = await volunteerQuery.ToPagedList(
            query.Page,
            query.PageSize,
            cancellationToken);
        
        _logger.LogInformation(
            "Get volunteers with pagination Page: {Page}, PageSize: {PageSize}, TotalCount: {TotalCount}",
            pagedList.Page, pagedList.PageSize, pagedList.TotalCount);

        return pagedList;
    }

    private static Expression<Func<VolunteerDto, object>> SortByProperty(GetFilteredVolunteersWithPaginationQuery query)
    {
        Expression<Func<VolunteerDto, object>> keySelector = query.SortBy?.ToLower() switch
        {
            "name" => (volunteer) => volunteer.FirstName,
            "surname" => (volunteer) => volunteer.SecondName,
            "patronymic" => (volunteer) => volunteer.Patronymic,
            "age" => (volunteer) => volunteer.WorkExperience,
            _ => (volunteer) => volunteer.Id
        };
        return keySelector;
    }

    private static IQueryable<VolunteerDto> VolunteerQueryFilter(GetFilteredVolunteersWithPaginationQuery query,
        IQueryable<VolunteerDto> volunteerQuery)
    {
        volunteerQuery = volunteerQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.FirstName),
            v => v.FirstName.Contains(query.FirstName!));
        
        volunteerQuery = volunteerQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.SecondName),
            v => v.SecondName.Contains(query.SecondName!));
        
        volunteerQuery = volunteerQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Patronymic),
            v => v.Patronymic.Contains(query.Patronymic!));
        
        volunteerQuery = volunteerQuery.WhereIf(
            query.WorkExperienceFrom != null,
            v => v.WorkExperience >= query.WorkExperienceFrom);
        
        volunteerQuery = volunteerQuery.WhereIf(
            query.WorkExperienceTo != null,
            v => v.WorkExperience <= query.WorkExperienceTo);
        return volunteerQuery;
    }
}

public class GetFilteredVolunteersWithPaginationHandlerDapper :
    IQueryHandler<PagedList<VolunteerDto>, GetFilteredVolunteersWithPaginationQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<GetVolunteersWithPaginationHandler> _logger;

    public GetFilteredVolunteersWithPaginationHandlerDapper(
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
        
        var sql = new StringBuilder("""
                                    select 
                                        id,
                                        first_name,
                                        second_name,
                                        patronymic,
                                        description,
                                        email,
                                        phone_number,
                                        work_experience,
                                        requisites,
                                        social_networks
                                        from volunteers
                                    """);

        var validProperties = new[]
        {
            "first_name",
            "second_name",
            "patronymic",
            "work_experience"
        };
        
        sql.ApplySorting(query.SortBy,query.SortDirection,validProperties);
        
        sql.ApplyFilterByString(validProperties[0], query.FirstName, validProperties);
        sql.ApplyFilterByString(validProperties[1], query.SecondName, validProperties);
        sql.ApplyFilterByString(validProperties[2], query.Patronymic, validProperties);
        
        if(query.WorkExperienceFrom != null && query.WorkExperienceTo != null)
            sql.ApplyFilterByNumber<int>(validProperties[3], (int)query.WorkExperienceFrom, (int)query.WorkExperienceTo ,validProperties);
        
        sql.ApplyPagination(query.Page,query.PageSize);

        var volunteers = 
            await connection.QueryAsync<VolunteerDto, string, string, VolunteerDto>(
                sql.ToString(),
                (volunteer, jsonRequisites, jsonSocialNetworks) =>
                {
                    var requisites = JsonSerializer.Deserialize<RequisiteDto[]>(jsonRequisites) ?? [];
                    volunteer.Requisites = requisites;

                    var socialNetworks = JsonSerializer.Deserialize<SocialNetworkDto[]>(jsonSocialNetworks) ?? [];
                    volunteer.SocialNetworks = socialNetworks;
                    
                    return volunteer;
                },
                splitOn:"requisites, social_networks",
                param: parameters);
        
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