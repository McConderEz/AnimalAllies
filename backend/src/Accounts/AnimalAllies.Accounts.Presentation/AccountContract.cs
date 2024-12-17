using AnimalAllies.Accounts.Application.AccountManagement.Queries.GetBannedUserById;
using AnimalAllies.Accounts.Application.AccountManagement.Queries.GetPermissionsByUserId;
using AnimalAllies.Accounts.Application.AccountManagement.Queries.IsUserExistById;
using AnimalAllies.Accounts.Application.Managers;
using AnimalAllies.Accounts.Contracts;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.Core.DTOs.Accounts;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Errors;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AnimalAllies.Accounts.Presentation;

public class AccountContract: IAccountContract
{
    private readonly GetPermissionsByUserIdHandler _getPermissionsByUserIdHandler;
    private readonly IsUserExistByIdHandler _isUserExistByIdHandler;
    private readonly GetBannedUserByIdHandler _getBannedUserByIdHandler;
    private readonly IAccountManager _accountManager;
    private readonly UserManager<User> _userManager;
    
    public AccountContract(
        GetPermissionsByUserIdHandler getPermissionsByUserIdHandler,
        IsUserExistByIdHandler isUserExistByIdHandler,
        GetBannedUserByIdHandler getBannedUserByIdHandler,
        IAccountManager accountManager, UserManager<User> userManager)
    {
        _getPermissionsByUserIdHandler = getPermissionsByUserIdHandler;
        _isUserExistByIdHandler = isUserExistByIdHandler;
        _getBannedUserByIdHandler = getBannedUserByIdHandler;
        _accountManager = accountManager;
        _userManager = userManager;
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
    

    public async Task<Result<ProhibitionSendingDto>> GetBannedUserById(
        Guid userId, CancellationToken cancellationToken = default)
    {
        return await _getBannedUserByIdHandler.Handle(userId, cancellationToken);
    }
    

    public async Task<Result> CreateVolunteerAccount(
        Guid userId, VolunteerInfo volunteerInfo, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user is null)
            return Errors.General.NotFound(userId);

        var volunteer = new VolunteerAccount(
            volunteerInfo.FullName, 
            volunteerInfo.WorkExperience.Value,
            user);
        user.VolunteerAccount = volunteer;

        return await _accountManager.CreateVolunteerAccount(volunteer, cancellationToken);
    }
}