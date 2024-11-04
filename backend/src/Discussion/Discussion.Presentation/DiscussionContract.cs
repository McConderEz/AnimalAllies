using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using Discussion.Application.Features.Commands;
using Discussion.Application.Features.Commands.CreateDiscussion;
using Discussion.Contracts;

namespace Discussion.Presentation;

public class DiscussionContract: IDiscussionContract
{
    private readonly CreateDiscussionHandler _createDiscussionHandler;

    public DiscussionContract(CreateDiscussionHandler createDiscussionHandler)
    {
        _createDiscussionHandler = createDiscussionHandler;
    }

    public async Task<Result<DiscussionId>> CreateDiscussionHandler(
        Guid firstMember, 
        Guid secondMember,
        Guid relationId,
        CancellationToken cancellationToken = default)
    {
        return await _createDiscussionHandler.Handle(firstMember, secondMember, relationId, cancellationToken);
    }
}