using AnimalAllies.Application.Abstractions;

namespace AnimalAllies.Application.Features.Species.Commands.CreateSpecies;

public record CreateSpeciesCommand(string Name) : ICommand;
