using AnimalAllies.Species.Application.SpeciesManagement.Commands.CreateSpecies;

namespace AnimalAllies.Species.Presentation.Requests;

public record CreateSpeciesRequest(string Name)
{
    public CreateSpeciesCommand ToCommand()
        => new(Name);
}