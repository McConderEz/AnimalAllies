using System.Data;
using System.Text;
using System.Text.Json;
using AnimalAllies.Accounts.Domain;
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

public class GetUserByIdHandler: IQueryHandler<UserDto?, GetUserByIdQuery>
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

    public async Task<Result<UserDto?>> Handle(
        GetUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();
        
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        
        parameters.Add("@UserId", query.UserId);

        var user = (await GetUser(connection, parameters)).FirstOrDefault();

        if(user!.Roles.Any(r => r.Name == ParticipantAccount.Participant))
            user.ParticipantAccount = (await GetParticipantAccount(connection, parameters)).FirstOrDefault();

        if(user.Roles.Any(r => r.Name == VolunteerAccount.Volunteer))
            user.VolunteerAccount = (await GetVolunteerAccount(connection, parameters)).FirstOrDefault();
        
        return user;
    }

    private static async Task<IEnumerable<VolunteerAccountDto>> GetVolunteerAccount(IDbConnection connection, DynamicParameters parameters)
    {
        var sqlForVolunteerAccount = new StringBuilder("""
                                                       select
                                                            id as volunteer_id,
                                                            user_id,
                                                            experience,
                                                            first_name,
                                                            second_name,
                                                            patronymic,
                                                            certificates,
                                                            requisites
                                                            from accounts.volunteer_accounts
                                                            where user_id = @UserId
                                                       """);

        var volunteerAccount = await connection
            .QueryAsync<VolunteerAccountDto, string, string, VolunteerAccountDto>(
                sqlForVolunteerAccount.ToString(),
                (volunteer, jsonCertificates, jsonRequisites) =>
                {
                    var certificates = JsonSerializer
                        .Deserialize<CertificateDto[]>(jsonCertificates) ?? [];
                    volunteer.Certificates = certificates;
                    
                    var requisites = JsonSerializer
                        .Deserialize<RequisiteDto[]>(jsonRequisites) ?? [];
                    volunteer.Requisites = requisites;
                    
                    return volunteer;
                },
                splitOn:"certificates,requisites",
                param: parameters
            );
        return volunteerAccount;
    }

    private static async Task<IEnumerable<ParticipantAccountDto>> GetParticipantAccount(IDbConnection connection, DynamicParameters parameters)
    {
        var sqlForParticipantAccount = new StringBuilder("""
                                                         select
                                                              id as participant_id,
                                                              user_id,
                                                              first_name,
                                                              second_name,
                                                              patronymic
                                                              from accounts.participant_accounts
                                                              where user_id = @UserId
                                                         """);

        var participantAccount = await connection
            .QueryAsync<ParticipantAccountDto>(sqlForParticipantAccount.ToString(), parameters);
        return participantAccount;
    }

    private static async Task<IEnumerable<UserDto>> GetUser(IDbConnection connection, DynamicParameters parameters)
    {
        var sql = new StringBuilder("""
                                    select
                                        u.id ,
                                        u.user_name,
                                        u.photo,
                                        r.id as role_id,
                                        r.name as name,
                                        u.social_networks as social_networks
                                    from accounts.users u
                                             join accounts.role_user ru on u.id = ru.users_id
                                             join accounts.roles r on ru.roles_id = r.id
                                    where u.id = @UserId
                                    """);

        var user = await connection
            .QueryAsync<UserDto, RoleDto,string, UserDto>(
                sql.ToString(),
                (user, role, jsonSocialNetworks) =>
                {
                    var socialNetworks = JsonSerializer
                        .Deserialize<SocialNetworkDto[]>(jsonSocialNetworks) ?? [];
                    user.SocialNetworks = socialNetworks;

                    user.Roles = [role];
                    
                    return user;
                },
                param: parameters,
                splitOn: "role_id, social_networks"
            );
        return user;
    }
}