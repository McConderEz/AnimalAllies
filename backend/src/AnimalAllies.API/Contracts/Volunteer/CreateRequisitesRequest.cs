using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Application.Features.Volunteer.Commands.CreateRequisites;

namespace AnimalAllies.API.Contracts.Volunteer;

public record CreateRequisitesRequest(IEnumerable<RequisiteDto> Requisites)
{
    public CreateRequisitesCommand ToCommand(Guid volunteerId)
        => new(volunteerId, Requisites);
}