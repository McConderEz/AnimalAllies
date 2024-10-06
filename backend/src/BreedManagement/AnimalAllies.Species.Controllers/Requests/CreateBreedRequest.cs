
using AnimalAllies.Species.Application.SpeciesManagement.Commands.CreateBreed;

namespace AnimalAllies.Species.Controllers.Requests;

public record CreateBreedRequest(string Name)
{
    public CreateBreedCommand ToCommand(Guid speciesId)
        => new(speciesId, Name);
}