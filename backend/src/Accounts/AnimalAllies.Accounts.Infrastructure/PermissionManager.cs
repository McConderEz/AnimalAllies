using AnimalAllies.Accounts.Domain;
using Microsoft.EntityFrameworkCore;

namespace AnimalAllies.Accounts.Infrastructure;

public class PermissionManager(AccountsDbContext accountsDbContext)
{
    public async Task AddRangeIfExist(IEnumerable<string> permissions)
    {
        foreach (var permissionCode in permissions)
        {
            var isPermissionExist = await accountsDbContext.Permissions
                .AnyAsync(p => p.Code == permissionCode);
        
            if(isPermissionExist)
                return;

            await accountsDbContext.Permissions.AddAsync(new Permission { Code = permissionCode });
        }

        await accountsDbContext.SaveChangesAsync();
    }
}