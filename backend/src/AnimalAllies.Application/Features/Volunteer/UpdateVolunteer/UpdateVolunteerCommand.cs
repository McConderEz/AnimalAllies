using AnimalAllies.Application.Contracts.DTOs;

namespace AnimalAllies.Application.Features.Volunteer.UpdateVolunteer;

public record UpdateVolunteerCommand(Guid Id, UpdateVolunteerMainInfoDto Dto);