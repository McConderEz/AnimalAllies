using Microsoft.AspNetCore.Authorization;

namespace AnimalAllies.Accounts.Infrastructure;

public class PermissionRequirementHandler: AuthorizationHandler<PermissionRequirement>
{
    protected async override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var permission = context.User.Claims.FirstOrDefault(c => c.Type == "Permission");
        if(permission is null)
            return;
        
        if(permission.Value == "create")
            context.Succeed(requirement);
    }
}