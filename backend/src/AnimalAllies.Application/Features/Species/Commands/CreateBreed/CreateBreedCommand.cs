using AnimalAllies.Application.Abstractions;

namespace AnimalAllies.Application.Features.Species.Commands.CreateBreed;

public record CreateBreedCommand(Guid SpeciesId, string Name) : ICommand;
