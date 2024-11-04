using AnimalAllies.Core.DTOs.ValueObjects;

namespace VolunteerRequests.Contracts.Requests;

public record CreateVolunteerRequestRequest(
    FullNameDto FullNameDto,
    string Email,
    string PhoneNumber,
    int WorkExperience,
    string VolunteerDescription,
    IEnumerable<SocialNetworkDto> SocialNetworkDtos);
