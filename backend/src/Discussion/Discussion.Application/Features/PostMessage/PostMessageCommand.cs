using AnimalAllies.Core.Abstractions;

namespace Discussion.Application.Features.PostMessage;

public record PostMessageCommand(Guid DiscussionId, Guid UserId, string Text) : ICommand;
