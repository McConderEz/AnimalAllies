using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;

namespace AnimalAllies.Accounts.Domain;

public class BannedUser: Entity<BannedUserId>
{
    private BannedUser(BannedUserId id) : base(id) { } 

    private BannedUser(BannedUserId id, Guid userId, Guid relationId, DateTime bannedAt) : base(id)
    {
        UserId = userId;
        RelationId = relationId;
        BannedAt = bannedAt;
    }

    public Guid UserId { get; private set; }
    public Guid RelationId { get; private set; }
    public DateTime BannedAt { get; private set; }

    public static Result<BannedUser> Create(BannedUserId bannedUserId, Guid userId,Guid relationId, DateTime bannedAt)
    {
        if (bannedUserId.Id == Guid.Empty)
            return Errors.General.ValueIsRequired("banned user id");
        
        if (userId == Guid.Empty)
            return Errors.General.ValueIsRequired("user id");
        
        if (relationId == Guid.Empty)
            return Errors.General.ValueIsRequired("relation id");

        if (bannedAt > DateTime.Now)
            return Errors.General.ValueIsInvalid("banned at");

        return new BannedUser(bannedUserId, userId,relationId, bannedAt);
    }
    
}