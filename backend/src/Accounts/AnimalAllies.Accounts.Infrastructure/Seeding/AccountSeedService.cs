using System.Text.Json;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.Accounts.Infrastructure.IdentityManagers;
using AnimalAllies.Accounts.Infrastructure.Options;
using AnimalAllies.Framework;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AnimalAllies.Accounts.Infrastructure.Seeding;

public class AccountSeedService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly PermissionManager _permissionManager;
    private readonly RolePermissionManager _rolePermissionManager;
    private readonly AccountManager _accountManager;
    private readonly AdminOptions _adminOptions;
    private readonly ILogger<AccountSeedService> _logger;

    public AccountSeedService(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        PermissionManager permissionManager,
        RolePermissionManager rolePermissionManager,
        IOptions<AdminOptions> adminOptions, 
        ILogger<AccountSeedService> logger,
        AccountManager accountManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _permissionManager = permissionManager;
        _rolePermissionManager = rolePermissionManager;
        _logger = logger;
        _accountManager = accountManager;
        _adminOptions = adminOptions.Value;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Seeding accounts...");

        var json = await File.ReadAllTextAsync(FilePaths.Accounts);
        
        var seedData = JsonSerializer.Deserialize<RolePermissionOptions>(json)
                       ?? throw new ApplicationException("Could not deserialize role permission config.");

        await SeedPermissions(seedData);

        await SeedRoles(seedData);

        await SeedRolePermissions(seedData);

        await CreateAdmin();
    }

    private async Task CreateAdmin()
    {
        var adminRole = await _roleManager.FindByNameAsync(AdminProfile.ADMIN)
                        ?? throw new ApplicationException("Could not find admin role");

        var adminUser = User.CreateAdmin(_adminOptions.UserName, _adminOptions.Email, adminRole);

        var isAdminExist = await _userManager.FindByNameAsync(AdminProfile.ADMIN);
        if(isAdminExist is not null)
            return;

        adminUser.EmailConfirmed = true;
        
        await _userManager.CreateAsync(adminUser, _adminOptions.Password);

        var fullName = FullName.Create(
            _adminOptions.UserName,
            _adminOptions.UserName, 
            _adminOptions.UserName);
        
        var adminProfile = new AdminProfile(fullName.Value, adminUser);

        await _accountManager.CreateAdminAccount(adminProfile);
    }

    private  async Task SeedRolePermissions(RolePermissionOptions seedData)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            await _rolePermissionManager.AddRangeIfExist(role!.Id, seedData.Roles[roleName]);
        }
        
        _logger.LogInformation("RolePermission added to database");
    }

    private async Task SeedRoles(RolePermissionOptions seedData)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var existingRole = await _roleManager.FindByNameAsync(roleName);

            if (existingRole is null)
                await _roleManager.CreateAsync(new Role { Name = roleName });
        }
        
        _logger.LogInformation("Roles added to database");
    }

    private async Task SeedPermissions(RolePermissionOptions seedData)
    {
        var permissionsToAdd = seedData.Permissions
            .SelectMany(permissionGroup => permissionGroup.Value);

        await _permissionManager.AddRangeIfExist(permissionsToAdd);
        
        _logger.LogInformation("Permissions added to database");
    }
}