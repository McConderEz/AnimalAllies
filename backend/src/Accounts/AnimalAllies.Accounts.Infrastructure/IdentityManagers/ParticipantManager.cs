using AnimalAllies.Accounts.Application.Managers;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.SharedKernel.Shared;

namespace AnimalAllies.Accounts.Infrastructure.IdentityManagers;

public class ParticipantManager(AccountsDbContext accountsDbContext) : IParticipantManager
{
    public async Task<Result> CreateParticipantAccount(
        ParticipantAccount participantAccount, CancellationToken cancellationToken = default)
    {
        await accountsDbContext.ParticipantAccounts.AddAsync(participantAccount,cancellationToken);
        await accountsDbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}