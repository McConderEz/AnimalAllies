using AnimalAllies.Accounts.Application.Managers;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Accounts.Application.AccountManagement.Queries.GetPermissionsByUserId;

public class GetPermissionsByUserIdHandler: IQueryHandler<List<string>, GetPermissionsByUserIdQuery>
{
    private readonly IPermissionManager _permissionManager;
    private readonly ILogger<GetPermissionsByUserIdHandler> _logger;
    private readonly IValidator<GetPermissionsByUserIdQuery> _validator;

    public GetPermissionsByUserIdHandler(
        ILogger<GetPermissionsByUserIdHandler> logger,
        IValidator<GetPermissionsByUserIdQuery> validator,
        IPermissionManager permissionManager)
    {
        _logger = logger;
        _validator = validator;
        _permissionManager = permissionManager;
    }
    
    public async Task<Result<List<string>>> Handle(
        GetPermissionsByUserIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();

        var result = await _permissionManager.GetPermissionsByUserId(query.UserId, cancellationToken);
        if (result.IsFailure)
            return result.Errors;

        return result.Value;
    }
}