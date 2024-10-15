

using AnimalAllies.Accounts.Domain;
using AnimalAllies.SharedKernel.Shared;

namespace AnimalAllies.Accounts.Contracts;

public interface IAccountContract
{
    Task<Result<List<string>>> GetPermissionsByUserId(Guid id, CancellationToken cancellationToken = default);
}