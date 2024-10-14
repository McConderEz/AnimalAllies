using AnimalAllies.Accounts.Domain;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Accounts.Application.AccountManagement.Queries.GetPermissionsByUserId;

public class GetPermissionsByUserIdHandler: IQueryHandler<List<Permission>, GetPermissionsByUserIdQuery>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<GetPermissionsByUserIdHandler> _logger;
    private readonly IValidator<GetPermissionsByUserIdQuery> _validator;

    public GetPermissionsByUserIdHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ILogger<GetPermissionsByUserIdHandler> logger,
        IValidator<GetPermissionsByUserIdQuery> validator)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result<List<Permission>>> Handle(
        GetPermissionsByUserIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();
        
        var isUserExist = await _userManager.FindByIdAsync(query.UserId.ToString());
        if (isUserExist is null)
            return Errors.General.NotFound();

        var roles = await _userManager.GetRolesAsync(isUserExist);
        
        var permissions = new List<Permission>();

        foreach (var role in roles)
        {
            var roleEntity = await _roleManager.FindByNameAsync(role);
            if (roleEntity != null)
            {
                var permissionOfRole = roleEntity.RolePermissions.Select(rp => rp.Permission);
                permissions.AddRange(permissionOfRole);
            }

        }

        return permissions.Distinct().ToList();
    }
}