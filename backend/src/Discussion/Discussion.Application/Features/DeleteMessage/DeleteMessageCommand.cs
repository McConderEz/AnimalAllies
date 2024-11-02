using AnimalAllies.Core.Abstractions;

namespace Discussion.Application.Features.DeleteMessage;

public record DeleteMessageCommand(Guid DiscussionId, Guid UserId, Guid MessageId) : ICommand;
