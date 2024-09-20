using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Features.Volunteer.Commands.AddPet;

namespace AnimalAllies.Application.Features.Volunteer.Commands.DeleteVolunteer;

public record DeleteVolunteerCommand(Guid Id) : ICommand;