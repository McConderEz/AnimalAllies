using AnimalAllies.Core.DTOs.ValueObjects;
using AnimalAllies.SharedKernel.Shared;

namespace VolunteerRequests.Contracts.Messaging;

public record ApprovedVolunteerRequestEvent(
    Guid UserId,
    string FirstName,
    string SecondName,
    string? Patronymic,
    int WorkExperience) : IDomainEvent;
