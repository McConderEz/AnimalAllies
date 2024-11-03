using AnimalAllies.Core.Abstractions;

namespace Discussion.Application.Features.CloseDiscussion;

public record CloseDiscussionCommand(Guid DiscussionId, Guid UserId) : ICommand;
