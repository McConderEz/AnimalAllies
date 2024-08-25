
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Domain.Models;

namespace AnimalAllies.Application.Features.Volunteer;

public record UpdateVolunteerRequest(
    Guid Id,
    FullNameDto FullName,
    string Email,
    string Description,
    int WorkExperience,
    string PhoneNumber);