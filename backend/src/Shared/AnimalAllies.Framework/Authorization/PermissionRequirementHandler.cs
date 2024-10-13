using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalAllies.Framework.Authorization;

public class PermissionRequirementHandler: AuthorizationHandler<PermissionAttribute>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionRequirementHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    protected async override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute permission)
    {
        
        
        
        context.Succeed(permission);
    }
}