using AnimalAllies.Accounts.Application.Managers;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using Microsoft.EntityFrameworkCore;

namespace AnimalAllies.Accounts.Infrastructure.IdentityManagers;

public class BanManager(AccountsDbContext context): IBanManager
{
    public async Task<Result> BanUser(BannedUser bannedUser, CancellationToken cancellationToken = default)
    {
        await context.BannedUsers.AddAsync(bannedUser, cancellationToken);

        return Result.Success();
    }

    public async Task<Result<BannedUser?>> GetBannedUserById(
        Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.BannedUsers.FirstOrDefaultAsync(b => b.UserId == userId, cancellationToken);
    }

    public Result DeleteBannedUser(BannedUser bannedUser, CancellationToken cancellationToken = default)
    {
        context.BannedUsers.Remove(bannedUser);

        return Result.Success();
    }
}