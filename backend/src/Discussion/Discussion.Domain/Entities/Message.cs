using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using Discussion.Domain.ValueObjects;

namespace Discussion.Domain.Entities;

public class Message: Entity<MessageId>
{
    private Message(MessageId id) : base(id){}

    public Message(MessageId id, Text text, CreatedAt createdAt, IsEdited isEdited, UserId userId) : base(id)
    {
        Text = text;
        CreatedAt = createdAt;
        IsEdited = isEdited;
        UserId = userId;
    }
    
    public Text Text { get; private set; }
    public CreatedAt CreatedAt { get; private set; }
    public IsEdited IsEdited { get; private set; }
    public UserId UserId { get; private set; }
    
}