using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using VolunteerRequests.Domain.ValueObjects;

namespace VolunteerRequests.Domain.Aggregate;

public class VolunteerRequest: Entity<VolunteerRequestId>
{
    private VolunteerRequest(VolunteerRequestId id) : base(id){}

    public VolunteerRequest(
        VolunteerRequestId id,
        CreatedAt createdAt,
        VolunteerInfo volunteerInfo,
        Guid userId) 
    : base(id)
    {
        CreatedAt = createdAt;
        VolunteerInfo = volunteerInfo;
        UserId = userId;
        RequestStatus = RequestStatus.Waiting;
        RejectionComment = RejectionComment.Create(" ").Value;
    }

    public static Result<VolunteerRequest> Create(
        VolunteerRequestId id,
        CreatedAt createdAt,
        VolunteerInfo volunteerInfo,
        Guid userId)
    {
        if (userId == Guid.Empty)
            return Errors.General.Null("user id");

        return new VolunteerRequest(id, createdAt, volunteerInfo, userId);
    }
    
    public CreatedAt CreatedAt { get; private set; }
    public RequestStatus RequestStatus { get; private set; }
    public VolunteerInfo VolunteerInfo { get; private set; }
    public Guid AdminId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid DiscussionId { get; private set; }
    public RejectionComment RejectionComment { get; private set; }
    

    public Result TakeRequestForSubmit(Guid adminId, Guid discussionId)
    {
        if (RequestStatus != RequestStatus.Waiting)
            return Errors.General.ValueIsInvalid("volunteer request status");

        if (adminId == Guid.Empty || discussionId == Guid.Empty)
            return Errors.General.ValueIsRequired();
        
        RequestStatus = RequestStatus.Submitted;
        AdminId = adminId;
        DiscussionId = discussionId;
        return Result.Success();
    }

    public Result ResendVolunteerRequest()
    {
        if (RequestStatus != RequestStatus.RevisionRequired)
            return Errors.General.ValueIsInvalid("volunteer request status");
        
        RequestStatus = RequestStatus.Submitted;

        return Result.Success();
    }
    
    public Result SendRequestForRevision(RejectionComment rejectionComment)
    {
        if(RequestStatus != RequestStatus.Submitted)
            return Errors.General.ValueIsInvalid("volunteer request status");
        
        if (rejectionComment is null)
            return Errors.General.ValueIsRequired();
        
        RequestStatus = RequestStatus.RevisionRequired;
        RejectionComment = rejectionComment;
        
        return Result.Success();
    }
    
    public Result RejectRequest(RejectionComment rejectionComment)
    {
        if(RequestStatus != RequestStatus.Submitted)
            return Errors.General.ValueIsInvalid("volunteer request status");
        
        if (rejectionComment is null)
            return Errors.General.ValueIsRequired();
        
        RequestStatus = RequestStatus.Rejected;
        RejectionComment = rejectionComment;
        
        return Result.Success();;
    }

    public Result ApproveRequest()
    {
        if(RequestStatus != RequestStatus.Submitted)
            return Errors.General.ValueIsInvalid("volunteer request status");
        
        RequestStatus = RequestStatus.Approved;
        return Result.Success();
    }
    
}