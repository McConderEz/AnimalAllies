using AnimalAllies.Application.Contracts.DTOs;

namespace AnimalAllies.Application.Features.Volunteer;

public record CreateRequisitesRequest(Guid Id,IEnumerable<RequisiteDto> Requisites);
