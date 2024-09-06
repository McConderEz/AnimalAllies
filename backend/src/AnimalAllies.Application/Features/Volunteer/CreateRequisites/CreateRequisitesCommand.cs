using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;

namespace AnimalAllies.Application.Features.Volunteer.CreateRequisites;

public record CreateRequisitesCommand(Guid Id,IEnumerable<RequisiteDto> RequisiteDtos);
