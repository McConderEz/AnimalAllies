using AnimalAllies.Core.DTOs.ValueObjects;

namespace AnimalAllies.Core.DTOs.Accounts;

public class VolunteerAccountDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public FullNameDto FullName { get; set; }
    public RequisiteDto[] Requisites{ get; set; } = [];
    public CertificateDto[] Certificates { get; set; } = [];
    public int Experience { get; set; }
}