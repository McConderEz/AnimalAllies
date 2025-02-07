using AnimalAllies.Accounts.Application.Managers;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.Core.DTOs.Accounts;
using AnimalAllies.SharedKernel.Shared;

namespace AnimalAllies.Accounts.Infrastructure.IdentityManagers;

public class AccountManager(AccountsDbContext accountsDbContext) : IAccountManager
{
    public async Task CreateAdminAccount(AdminProfile adminProfile)
    {
        await accountsDbContext.AdminProfiles.AddAsync(adminProfile);
        await accountsDbContext.SaveChangesAsync();
    }
    
    public async Task<Result> CreateParticipantAccount(
        ParticipantAccount participantAccount, CancellationToken cancellationToken = default)
    {
        await accountsDbContext.ParticipantAccounts.AddAsync(participantAccount,cancellationToken);
        await accountsDbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> CreateVolunteerAccount(
        VolunteerAccount volunteerAccount, CancellationToken cancellationToken = default)
    {
        await accountsDbContext.VolunteerAccounts.AddAsync(volunteerAccount,cancellationToken);

        await accountsDbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}