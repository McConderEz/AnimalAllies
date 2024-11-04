namespace Discussion.Contracts.Requests;

public record UpdateMessageRequest(Guid DiscussionId, Guid MessageId, string Text);