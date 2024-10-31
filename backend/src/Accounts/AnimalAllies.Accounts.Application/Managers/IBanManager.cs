using AnimalAllies.Accounts.Domain;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;

namespace AnimalAllies.Accounts.Application.Managers;

public interface IBanManager
{
    Task<Result> BanUser(BannedUser bannedUser, CancellationToken cancellationToken = default);
    Task<Result<BannedUser?>> GetBannedUserById(Guid userId, CancellationToken cancellationToken = default);
    Result DeleteBannedUser(BannedUser bannedUser, CancellationToken cancellationToken = default);
}