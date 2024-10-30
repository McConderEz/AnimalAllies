using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;

namespace Discussion.Contracts;

public interface IDiscussionContract
{
    public Task<Result<DiscussionId>> CreateDiscussionHandler(
        Guid firstMember,
        Guid secondMember,
        Guid relationId,
        CancellationToken cancellationToken = default);
}