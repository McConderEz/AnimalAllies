using AnimalAllies.Application.Features.Species.Commands.CreateBreed;

namespace AnimalAllies.API.Contracts.Volunteer;

public record CreateBreedRequest(string Name)
{
    public CreateBreedCommand ToCommand(Guid speciesId)
        => new(speciesId, Name);
}