using Microsoft.AspNetCore.Authorization;

namespace AnimalAllies.Accounts.Infrastructure;

public class PermissionRequirement: AuthorizeAttribute, IAuthorizationRequirement
{
    public string Code { get; set; }

    public PermissionRequirement(string code) => Code = code;

}