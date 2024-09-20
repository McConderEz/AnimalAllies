using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Application.Features.Volunteer.Commands.AddPet;

namespace AnimalAllies.Application.Features.Volunteer.Commands.CreateRequisites;

public record CreateRequisitesCommand(Guid Id,IEnumerable<RequisiteDto> RequisiteDtos) : ICommand;
