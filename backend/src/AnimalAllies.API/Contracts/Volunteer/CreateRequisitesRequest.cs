using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Application.Features.Volunteer.CreateRequisites;

namespace AnimalAllies.API.Contracts;

public record CreateRequisitesRequest(IEnumerable<RequisiteDto> Requisites)
{
    public CreateRequisitesCommand ToCommand(Guid volunteerId)
        => new(volunteerId, Requisites);
}