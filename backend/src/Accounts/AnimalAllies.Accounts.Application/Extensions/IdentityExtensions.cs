using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Errors;
using Microsoft.AspNetCore.Identity;

namespace AnimalAllies.Accounts.Application.Extensions;

public static class IdentityExtensions
{
    public static ErrorList ToErrorList(this IEnumerable<IdentityError> identityErrors)
    {
        var errors = identityErrors.Select(ie => Error.Failure(ie.Code, ie.Description));
        
        return new ErrorList(errors);
    }
}