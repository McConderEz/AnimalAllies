using AnimalAllies.SharedKernel.Shared;

namespace VolunteerRequests.Domain.Events;

public record ApprovedVolunteerRequestDomainEvent(
    Guid UserId,
    string FirstName,
    string SecondName,
    string? Patronymic,
    int WorkExperience): IDomainEvent;
