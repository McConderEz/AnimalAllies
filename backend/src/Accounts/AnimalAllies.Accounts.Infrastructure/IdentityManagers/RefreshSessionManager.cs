using AnimalAllies.Accounts.Application.Managers;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Errors;
using Microsoft.EntityFrameworkCore;

namespace AnimalAllies.Accounts.Infrastructure.IdentityManagers;

public class RefreshSessionManager(AccountsDbContext accountsDbContext) : IRefreshSessionManager
{

    public async Task<Result<RefreshSession>> GetByRefreshToken(
        Guid refreshToken, CancellationToken cancellationToken = default)
    {
        var refreshSession = await accountsDbContext.RefreshSessions
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.RefreshToken == refreshToken, cancellationToken);

        if (refreshSession is null)
            return Errors.General.NotFound();

        return refreshSession;
    }
    
    public void Delete(RefreshSession refreshSession)
    {
        accountsDbContext.RefreshSessions.Remove(refreshSession);
    }
}