using AnimalAllies.Application.Contracts.DTOs;

namespace AnimalAllies.Application.Features.Volunteer.UpdateVolunteer;

public record UpdateVolunteerRequest(Guid Id, UpdateVolunteerMainInfoDto Dto);