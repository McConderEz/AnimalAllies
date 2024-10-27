using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using Discussion.Domain.Entities;

namespace Discussion.Domain.Aggregate;

public class Discussion: Entity<DiscussionId>
{
    private List<Message> _messages = [];
    
    private Discussion(DiscussionId id) : base(id){}
    
        
    
}