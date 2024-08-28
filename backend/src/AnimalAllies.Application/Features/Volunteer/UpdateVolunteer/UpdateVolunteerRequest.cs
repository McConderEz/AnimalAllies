using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;

namespace AnimalAllies.Application.Features.Volunteer.Update;

public record UpdateVolunteerRequest(Guid Id, UpdateVolunteerMainInfoDto Dto);