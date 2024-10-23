using AnimalAllies.Core.DTOs.ValueObjects;

namespace AnimalAllies.Core.DTOs.Accounts;

public class ParticipantAccountDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public FullNameDto FullName { get; set; }
}