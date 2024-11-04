using System.Text;
using System.Text.Json;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.DTOs;
using AnimalAllies.Core.DTOs.ValueObjects;
using AnimalAllies.Core.Extension;
using AnimalAllies.Core.Models;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VolunteerRequests.Domain.ValueObjects;

namespace VolunteerRequests.Application.Features.Queries.GetVolunteerRequestsInWaitingWithPagination;

public class GetVolunteerRequestsInWaitingWithPaginationHandler:
    IQueryHandler<PagedList<VolunteerRequestDto>, GetVolunteerRequestsInWaitingWithPaginationQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IValidator<GetVolunteerRequestsInWaitingWithPaginationQuery> _validator;
    private readonly ILogger<GetVolunteerRequestsInWaitingWithPaginationHandler> _logger;
    
    public GetVolunteerRequestsInWaitingWithPaginationHandler(
        IValidator<GetVolunteerRequestsInWaitingWithPaginationQuery> validator,
        ILogger<GetVolunteerRequestsInWaitingWithPaginationHandler> logger,
        [FromKeyedServices(Constraints.Context.VolunteerRequests)]ISqlConnectionFactory sqlConnectionFactory)
    {
        _validator = validator;
        _logger = logger;
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    
    public async Task<Result<PagedList<VolunteerRequestDto>>> Handle(
        GetVolunteerRequestsInWaitingWithPaginationQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        parameters.Add("@RequestStatus", RequestStatus.Waiting.Value);
        
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
                                        admin_id,
                                        user_id,
                                        discussion_id,
                                        request_status,
                                        rejection_comment,
                                        social_networks
                                        from volunteer_requests.volunteer_requests 
                                        where request_status = @RequestStatus
                                    """);
        
        
        sql.ApplySorting(query.SortBy,query.SortDirection);
        
        sql.ApplyPagination(query.Page,query.PageSize);
        
        var volunteerRequests = 
            await connection.QueryAsync<VolunteerRequestDto, string, VolunteerRequestDto>(
                sql.ToString(),
                (volunteerRequest, jsonSocialNetworks) =>
                {
                    var socialNetworks = JsonSerializer
                        .Deserialize<SocialNetworkDto[]>(jsonSocialNetworks) ?? [];

                    volunteerRequest.SocialNetworks = socialNetworks;
                    return volunteerRequest;
                },
                splitOn:"social_networks",
                param: parameters);
        
        _logger.LogInformation("Get volunteer requests with pagination Page: {Page}, PageSize: {PageSize}",
            query.Page, query.PageSize);

        var volunteerRequestDtos = volunteerRequests.ToList();
        
        return new PagedList<VolunteerRequestDto>
        {
            Items = volunteerRequestDtos,
            PageSize = query.PageSize,
            Page = query.Page,
            TotalCount = volunteerRequestDtos.Count()
        };
    }
}