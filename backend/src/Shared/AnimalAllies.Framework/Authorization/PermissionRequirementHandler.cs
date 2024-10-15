using AnimalAllies.Accounts.Contracts;
using AnimalAllies.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalAllies.Framework.Authorization;

public class PermissionRequirementHandler(
    IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<PermissionAttribute>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute permission)
    {
        using var scope = serviceScopeFactory.CreateScope();

        var accountContract = scope.ServiceProvider.GetRequiredService<IAccountContract>();
        var userIdString = context.User.Claims
             .FirstOrDefault(claim => claim.Type == CustomClaims.Id)?.Value;
        if (!Guid.TryParse(userIdString, out var userId))
        {
            context.Fail();
            return;
        }

        var permissions = await accountContract.GetPermissionsByUserId(userId);
        if (permissions.IsFailure)
            context.Fail();

        if (permissions.Value.Contains(permission.Code))
        {
            context.Succeed(permission);
            return;
        }
        
        context.Fail();
    }
}