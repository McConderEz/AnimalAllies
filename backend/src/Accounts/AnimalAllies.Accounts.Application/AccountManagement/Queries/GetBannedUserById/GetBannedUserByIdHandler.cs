using System.Text;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.DTOs.Accounts;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Errors;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Accounts.Application.AccountManagement.Queries.GetBannedUserById;

public class GetBannedUserByIdHandler: IQueryHandler<ProhibitionSendingDto,GetBannedUserByIdQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<GetBannedUserByIdHandler> _logger;
    private readonly IValidator<GetBannedUserByIdQuery> _validator;

    public GetBannedUserByIdHandler(
        [FromKeyedServices(Constraints.Context.Accounts)]ISqlConnectionFactory sqlConnectionFactory,
        ILogger<GetBannedUserByIdHandler> logger, 
        IValidator<GetBannedUserByIdQuery> validator)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<ProhibitionSendingDto>> Handle(
        GetBannedUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var connection = _sqlConnectionFactory.Create();
        
        var parameters = new DynamicParameters();
        
        parameters.Add("@UserId", query.UserId);
        
       var sql = new StringBuilder("""
                                   select
                                       u.id,
                                       u.user_id,
                                       u.banned_at
                                   from accounts.banned_users u
                                   where u.user_id = @UserId
                                   """);

       var bannedUser = (await connection
           .QueryAsync<ProhibitionSendingDto>(sql.ToString(), parameters)).ToList();

       var result = bannedUser.SingleOrDefault();

       if (result is null)
           return Errors.General.NotFound();
       
       _logger.LogInformation("got user with id {id}", query.UserId);

        return result;
    }

    public async Task<Result<ProhibitionSendingDto>> Handle(Guid userId, CancellationToken cancellationToken = default)
    {
        return await Handle(new GetBannedUserByIdQuery(userId), cancellationToken);
    }
}