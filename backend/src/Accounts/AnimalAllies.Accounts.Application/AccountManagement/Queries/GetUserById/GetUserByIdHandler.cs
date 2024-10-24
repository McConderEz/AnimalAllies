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

        var user = await GetUser(connection, parameters);

        var participantAccount = await GetParticipantAccount(connection, parameters);

        var volunteerAccount = await GetVolunteerAccount(connection, parameters);

        var roles = await GetRoles(connection);
        
        var roleUser = await GetRoleUser(connection, parameters);
        
        var result = user.FirstOrDefault();

        if (participantAccount.FirstOrDefault() is not null)
            result.ParticipantAccountDto = participantAccount.FirstOrDefault();

        if (volunteerAccount.FirstOrDefault() is not null)
            result.VolunteerAccount = volunteerAccount.FirstOrDefault();
        
        result.Roles = roles.Where(r => roleUser.Any(ru => ru.RolesId == r.Id)).ToArray();

        return result;
    }

    private static async Task<IEnumerable<RoleUserDto>> GetRoleUser(IDbConnection connection, DynamicParameters parameters)
    {
        var sqlForRoleUser = new StringBuilder("""
                                                 select
                                                    roles_id, 
                                                    users_id
                                                    from accounts.role_user
                                                    where users_id = @UserId
                                               """);

        var roleUser = await connection
            .QueryAsync<RoleUserDto>(sqlForRoleUser.ToString(), parameters);
        return roleUser;
    }

    private static async Task<IEnumerable<RoleDto>> GetRoles(IDbConnection connection)
    {
        var sqlForRoles = new StringBuilder("""
                                              select
                                                 id,
                                                 name
                                                 from accounts.roles
                                            """);

        var roles = await connection
            .QueryAsync<RoleDto>(sqlForRoles.ToString());
        return roles;
    }

    private static async Task<IEnumerable<VolunteerAccountDto>> GetVolunteerAccount(IDbConnection connection, DynamicParameters parameters)
    {
        var sqlForVolunteerAccount = new StringBuilder("""
                                                       select
                                                            id,
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
                                                              id,
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
                                        u.id,
                                        u.user_name,
                                        u.photo,
                                        u.social_networks as social_networks
                                    from accounts.users u
                                    where u.id = @UserId
                                    """);

        var user = await connection
            .QueryAsync<UserDto, string, UserDto>(
                sql.ToString(),
                (user,jsonSocialNetworks) =>
                {
                    var socialNetworks = JsonSerializer
                        .Deserialize<SocialNetworkDto[]>(jsonSocialNetworks) ?? [];
                    user.SocialNetworks = socialNetworks;
                    return user;
                },
                param: parameters,
                splitOn: "social_networks"
            );
        return user;
    }
}