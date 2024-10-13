using Microsoft.AspNetCore.Authorization;

namespace AnimalAllies.Framework.Authorization;

public class PermissionAttribute: AuthorizeAttribute, IAuthorizationRequirement
{
    public string Code { get; set; }

    public PermissionAttribute(string code) => Code = code;

}