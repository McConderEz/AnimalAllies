using AnimalAllies.Accounts.Contracts;
using AnimalAllies.Core.Models;
using AnimalAllies.Framework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalAllies.Framework.Authorization;

public class PermissionRequirementHandler(IHttpContextAccessor httpContextAccessor) 
    : AuthorizationHandler<PermissionAttribute>
{

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute permission)
    {
        if (context.User.Identity is null || !context.User.Identity.IsAuthenticated 
                                          || httpContextAccessor.HttpContext is null)
        {
            context.Fail();
            return;
        }

        var userScopedData = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<UserScopedData>();

        if (userScopedData.Permissions.Contains(permission.Code))
        {
            context.Succeed(permission);
            return;
        }

        context.Fail();
    }
}