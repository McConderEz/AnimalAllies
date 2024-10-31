

using AnimalAllies.Accounts.Domain;
using AnimalAllies.Core.DTOs.Accounts;
using AnimalAllies.SharedKernel.Shared;

namespace AnimalAllies.Accounts.Contracts;

public interface IAccountContract
{
    Task<Result<List<string>>> GetPermissionsByUserId(Guid id, CancellationToken cancellationToken = default);
    Task<Result<bool>> IsUserExistById(Guid userId, CancellationToken cancellationToken = default);
    Task<Result> BanUser(Guid userId, Guid relationId, CancellationToken cancellationToken = default);
    Task<Result<BannedUserDto>> GetBannedUserById(Guid userId, CancellationToken cancellationToken = default);
    Task<Result> DeleteBannedUser(Guid userId, CancellationToken cancellationToken = default);

}