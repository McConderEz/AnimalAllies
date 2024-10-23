using System.Text;
using System.Text.Json;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.DTOs.Accounts;
using AnimalAllies.Core.DTOs.ValueObjects;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Accounts.Application.AccountManagement.Queries.GetUserById;

public class GetUserByIdHandler: IQueryHandler<UserDto, GetUserByIdQuery>
{
    private readonly ILogger<GetUserByIdHandler> _logger;
    private readonly IValidator<GetUserByIdQuery> _validator;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetUserByIdHandler(
        ILogger<GetUserByIdHandler> logger,
        IValidator<GetUserByIdQuery> validator, 
        [FromKeyedServices(Constraints.Context.Accounts)]ISqlConnectionFactory sqlConnectionFactory)
    {
        _logger = logger;
        _validator = validator;
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<UserDto>> Handle(
        GetUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();
        
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        
        parameters.Add("@UserId", query.UserId);

        var sql = new StringBuilder("""
                                    select 
                                        u.id as user_id,
                                        u.user_name,
                                        u.photo,
                                        r.id as role_id,
                                        r.name as role_name,
                                        va.id as volunteer_account_id,
                                        va.first_name as first_name,
                                        va.second_name as second_name,
                                        va.patronymic as patronymic,
                                        va.experience,
                                        pa.id AS participant_account_id,
                                        pa.first_name  as participant_first_name,
                                        pa.second_name as participant_second_name,
                                        pa.patronymic as participant_patronymic,
                                        u.social_networks as social_networks
                                    from accounts.users u
                                    left join accounts.user_roles ur ON u.id = ur.user_id
                                    left join accounts.roles r ON ur.role_id = r.id
                                    left join accounts.volunteer_accounts va ON u.id = va.user_id
                                    left join accounts.participant_accounts pa ON u.id = pa.user_id
                                    where u.id = @UserId
                                    """);
        


        var user = await connection
            .QueryAsync<UserDto, RoleDto, VolunteerAccountDto, ParticipantAccountDto, string, UserDto>(
            sql.ToString(),
            (user, role, volunteer, participant,jsonSocialNetworks) =>
            {
                var socialNetworks = JsonSerializer.Deserialize<SocialNetworkDto[]>(jsonSocialNetworks) ?? [];
                user.SocialNetworks = socialNetworks;

                return user;
            },
            param: parameters,
            splitOn: "role_id, volunteer_account_id, participant_account_id,social_networks"
        );
        
        return user.FirstOrDefault();
    }
}