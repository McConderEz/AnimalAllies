using AnimalAllies.Core.DTOs.Accounts;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.ValueObjects;

namespace AnimalAllies.Accounts.Contracts;

public interface IAccountContract
{
    Task<Result<bool>> IsUserExistById(Guid userId, CancellationToken cancellationToken = default);
}