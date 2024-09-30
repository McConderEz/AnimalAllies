using AnimalAllies.Application.Abstractions;

namespace AnimalAllies.Application.Features.Volunteer.Commands.UpdatePetStatus;

public record UpdatePetStatusCommand(Guid VolunteerId, Guid PetId, string HelpStatus) : ICommand;
