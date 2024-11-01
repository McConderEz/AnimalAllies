using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;

namespace VolunteerRequests.Domain.Aggregates;

public class ProhibitionSending: Entity<ProhibitionSendingId>
{
    private ProhibitionSending(ProhibitionSendingId id) : base(id) { } 

    private ProhibitionSending(ProhibitionSendingId id, Guid userId, DateTime bannedAt) : base(id)
    {
        UserId = userId;
        BannedAt = bannedAt;
    }

    public Guid UserId { get; private set; }
    public DateTime BannedAt { get; private set; }

    public static Result<ProhibitionSending> Create(ProhibitionSendingId prohibitionSendingId, Guid userId, DateTime bannedAt)
    {
        if (prohibitionSendingId.Id == Guid.Empty)
            return Errors.General.ValueIsRequired("banned user id");
        
        if (userId == Guid.Empty)
            return Errors.General.ValueIsRequired("user id");

        if (bannedAt > DateTime.Now)
            return Errors.General.ValueIsInvalid("banned at");

        return new ProhibitionSending(prohibitionSendingId, userId, bannedAt);
    }
    
}