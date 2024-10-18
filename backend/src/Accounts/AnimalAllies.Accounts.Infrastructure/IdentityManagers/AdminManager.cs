using AnimalAllies.Accounts.Domain;

namespace AnimalAllies.Accounts.Infrastructure.IdentityManagers;

public class AdminManager(AccountsDbContext accountsDbContext) 
{
    public async Task CreateAdminAccount(AdminProfile adminProfile)
    {
        await accountsDbContext.AdminProfiles.AddAsync(adminProfile);
        await accountsDbContext.SaveChangesAsync();
    }
}