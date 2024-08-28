using AnimalAllies.Application.Contracts.DTOs;

namespace AnimalAllies.Application.Features.Volunteer.CreateRequisites;

public record CreateRequisitesRequest(Guid Id,RequisiteListDto Dto);
