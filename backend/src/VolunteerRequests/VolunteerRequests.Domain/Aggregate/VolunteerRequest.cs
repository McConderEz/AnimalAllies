using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using VolunteerRequests.Domain.ValueObjects;

namespace VolunteerRequests.Domain.Aggregate;

public class VolunteerRequest: Entity<VolunteerRequestId>
{
    private VolunteerRequest(VolunteerRequestId id) : base(id){}

    public VolunteerRequest(
        VolunteerRequestId id,
        CreatedAt createdAt,
        VolunteerInfo volunteerInfo,
        UserId userId) 
    : base(id)
    {
        CreatedAt = createdAt;
        VolunteerInfo = volunteerInfo;
        UserId = userId;
    }
    
    public CreatedAt CreatedAt { get; private set; }
    public RequestStatus RequestStatus { get; private set; }
    public VolunteerInfo VolunteerInfo { get; private set; }
    public AdminId AdminId { get; private set; }
    public UserId UserId { get; private set; }
    public DiscussionId DiscussionId { get; private set; }
    public RejectionComment? RejectionComment { get; private set; }
    

    public Result TakeRequestForSubmit(AdminId adminId, DiscussionId discussionId)
    {
        RequestStatus = RequestStatus.Submitted;
        AdminId = adminId;
        DiscussionId = discussionId;
        return Result.Success();
    }
    
    public Result SendRequestForRevision(RejectionComment rejectionComment)
    {
        RequestStatus = RequestStatus.RevisionRequired;
        RejectionComment = rejectionComment;
        
        return Result.Success();
    }
    
    public Result RejectRequest(RejectionComment rejectionComment)
    {
        RequestStatus = RequestStatus.Rejected;
        RejectionComment = rejectionComment;
        
        return Result.Success();;
    }

    public Result ApproveRequest()
    {
        RequestStatus = RequestStatus.Approved;
        return Result.Success();
    }
    
}