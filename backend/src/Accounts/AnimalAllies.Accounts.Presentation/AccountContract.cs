using AnimalAllies.Accounts.Application.AccountManagement.Commands.BanUser;
using AnimalAllies.Accounts.Application.AccountManagement.Commands.DeleteBanUser;
using AnimalAllies.Accounts.Application.AccountManagement.Queries.GetBannedUserById;
using AnimalAllies.Accounts.Application.AccountManagement.Queries.GetPermissionsByUserId;
using AnimalAllies.Accounts.Application.AccountManagement.Queries.IsUserExistById;
using AnimalAllies.Accounts.Contracts;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.Core.DTOs.Accounts;
using AnimalAllies.SharedKernel.Shared;
using Microsoft.AspNetCore.Identity;

namespace AnimalAllies.Accounts.Presentation;

public class AccountContract: IAccountContract
{
    private readonly GetPermissionsByUserIdHandler _getPermissionsByUserIdHandler;
    private readonly IsUserExistByIdHandler _isUserExistByIdHandler;
    private readonly BanUserHandler _banUserHandler;
    private readonly GetBannedUserByIdHandler _getBannedUserByIdHandler;
    private readonly DeleteBannedUserHandler _deleteBannedUserHandler;
    
    public AccountContract(
        GetPermissionsByUserIdHandler getPermissionsByUserIdHandler,
        IsUserExistByIdHandler isUserExistByIdHandler,
        BanUserHandler banUserHandler, 
        GetBannedUserByIdHandler getBannedUserByIdHandler,
        DeleteBannedUserHandler deleteBannedUserHandler)
    {
        _getPermissionsByUserIdHandler = getPermissionsByUserIdHandler;
        _isUserExistByIdHandler = isUserExistByIdHandler;
        _banUserHandler = banUserHandler;
        _getBannedUserByIdHandler = getBannedUserByIdHandler;
        _deleteBannedUserHandler = deleteBannedUserHandler;
    }


    public async Task<Result<List<string>>> GetPermissionsByUserId(Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetPermissionsByUserIdQuery(id);
        
        var result = await _getPermissionsByUserIdHandler.Handle(query, cancellationToken);
        if (result.IsFailure)
            return result.Errors;
        
        return result.Value;
    }

    public async Task<Result<bool>> IsUserExistById(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _isUserExistByIdHandler.Handle(userId, cancellationToken);
    }

    public async Task<Result> BanUser(Guid userId, Guid relationId, CancellationToken cancellationToken = default)
    {
        return await _banUserHandler.Handle(userId, relationId, cancellationToken);
    }

    public async Task<Result<BannedUserDto>> GetBannedUserById(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _getBannedUserByIdHandler.Handle(userId, cancellationToken);
    }

    public async Task<Result> DeleteBannedUser(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _deleteBannedUserHandler.Handle(userId, cancellationToken);
    }
}