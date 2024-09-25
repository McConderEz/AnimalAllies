using AnimalAllies.Application.Abstractions;

namespace AnimalAllies.Application.Features.Species.Commands.DeleteBreed;

public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;
