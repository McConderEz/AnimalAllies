using System.Text.Json;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.Framework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Accounts.Infrastructure;

public class AccountsSeeder
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<AccountsSeeder> _logger;

    public AccountsSeeder(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<AccountsSeeder> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Seeding accounts...");
        
        var json = await File.ReadAllTextAsync(FilePaths.Accounts);

        using var scope = _serviceScopeFactory.CreateScope();
        
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        var permissionManager = scope.ServiceProvider.GetRequiredService<PermissionManager>();

        var seedData = JsonSerializer.Deserialize<RolePermissionConfig>(json)
            ?? throw new ApplicationException("Could not deserialize role permission config.");

        await SeedPermissions(seedData, permissionManager);
    }

    private async Task SeedPermissions(
        RolePermissionConfig seedData,
        PermissionManager permissionManager)
    {
        var permissionsToAdd = seedData.Permissions
            .SelectMany(permissionGroup => permissionGroup.Value);

        await permissionManager.AddRangeIfExist(permissionsToAdd);
        
        _logger.LogInformation("Permissions added to database");
    }
}

public class RolePermissionConfig
{
    public Dictionary<string, string[]> Permissions { get; set; } = [];
    public Dictionary<string, string[]> Roles { get; set; } = [];
}

