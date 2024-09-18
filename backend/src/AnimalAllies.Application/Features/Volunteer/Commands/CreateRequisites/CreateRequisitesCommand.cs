using AnimalAllies.Application.Contracts.DTOs.ValueObjects;

namespace AnimalAllies.Application.Features.Volunteer.Commands.CreateRequisites;

public record CreateRequisitesCommand(Guid Id,IEnumerable<RequisiteDto> RequisiteDtos);
