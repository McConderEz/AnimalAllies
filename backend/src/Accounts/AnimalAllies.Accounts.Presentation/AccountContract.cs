using AnimalAllies.Accounts.Application.AccountManagement.Queries.GetPermissionsByUserId;
using AnimalAllies.Accounts.Contracts;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.SharedKernel.Shared;
using Microsoft.AspNetCore.Identity;

namespace AnimalAllies.Accounts.Presentation;

public class AccountContract: IAccountContract
{
    private readonly GetPermissionsByUserIdHandler _getPermissionsByUserIdHandler;

    public AccountContract(GetPermissionsByUserIdHandler getPermissionsByUserIdHandler)
    {
        _getPermissionsByUserIdHandler = getPermissionsByUserIdHandler;
    }


    public async Task<Result<List<string>>> GetPermissionsByUserId(Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetPermissionsByUserIdQuery(id);
        
        var result = await _getPermissionsByUserIdHandler.Handle(query, cancellationToken);
        if (result.IsFailure)
            return result.Errors;
        
        return result.Value;
    }
}