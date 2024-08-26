using AnimalAllies.Application.Contracts.DTOs;

namespace AnimalAllies.Application.Features.Volunteer.Create;

public record CreateRequisitesRequest(Guid Id,RequisiteListDto Dto);
