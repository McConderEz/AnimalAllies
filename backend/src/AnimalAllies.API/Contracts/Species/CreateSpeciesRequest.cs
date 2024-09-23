using AnimalAllies.Application.Features.Species.Commands.CreateSpecies;

namespace AnimalAllies.API.Contracts.Volunteer;

public record CreateSpeciesRequest(string Name)
{
    public CreateSpeciesCommand ToCommand()
        => new(Name);
}