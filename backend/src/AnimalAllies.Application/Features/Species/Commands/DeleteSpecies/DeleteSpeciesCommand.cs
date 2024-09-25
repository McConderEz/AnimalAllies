using AnimalAllies.Application.Abstractions;

namespace AnimalAllies.Application.Features.Species.Commands.DeleteSpecies;

public record DeleteSpeciesCommand(Guid SpeciesId) : ICommand;