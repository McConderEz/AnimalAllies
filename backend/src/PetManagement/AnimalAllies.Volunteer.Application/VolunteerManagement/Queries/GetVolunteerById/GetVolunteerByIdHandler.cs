using System.Text;
using System.Text.Json;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.DTOs;
using AnimalAllies.Core.DTOs.ValueObjects;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Shared;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Queries.GetVolunteerById;

public class GetVolunteerByIdHandler : IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<GetVolunteerByIdHandler> _logger;
    private readonly IValidator<GetVolunteerByIdQuery> _validator;

    public GetVolunteerByIdHandler(
        ILogger<GetVolunteerByIdHandler> logger,
        ISqlConnectionFactory sqlConnectionFactory, IValidator<GetVolunteerByIdQuery> validator)
    {
        _logger = logger;
        _sqlConnectionFactory = sqlConnectionFactory;
        _validator = validator;
    }
    
    
    public async Task<Result<VolunteerDto>> Handle(
        GetVolunteerByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();
        
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        
        parameters.Add("@VolunteerId", query.VolunteerId);

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
                                    where id = @VolunteerId
                                    limit 1
                                    """);
        
        var volunteer = 
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

        
        var result = volunteer.FirstOrDefault();

        if (result is null) 
            return Errors.General.NotFound();
        
        _logger.LogInformation("Get volunteer with id {VolunteerId}", query.VolunteerId);
        
        return result;
    }
}