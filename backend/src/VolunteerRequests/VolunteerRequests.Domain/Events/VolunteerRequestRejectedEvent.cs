using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;

namespace VolunteerRequests.Domain.Events;

public record VolunteerRequestRejectedEvent(Guid UserId) : IDomainEvent;
