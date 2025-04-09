using AnimalAllies.Accounts.Domain;
using AnimalAllies.Core.Database;
using AnimalAllies.SharedKernel.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalAllies.Accounts.Infrastructure.IdentityManagers;

public class  RolePermissionManager(AccountsDbContext accountsDbContext,
    [FromKeyedServices(Constraints.Context.Accounts)] IUnitOfWork unitOfWork)
{
    public async Task AddRangeIfExist(Guid roleId,IEnumerable<string> permissions)
    {
        foreach (var permissionCode in permissions)
        {
            var permission = await accountsDbContext.Permissions
                .FirstOrDefaultAsync(p => p.Code == permissionCode);

            if (permission is null)
                throw new ApplicationException("permission is cannot be null");

            var rolePermissionExist = await accountsDbContext.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission!.Id);
                
            if(rolePermissionExist)
                continue;

            await accountsDbContext.RolePermissions.AddAsync(
                new RolePermission { RoleId = roleId, PermissionId = permission!.Id });
        }
        
        await unitOfWork.SaveChanges();
    }
}