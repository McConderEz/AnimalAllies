using CSharpFunctionalExtensions;

namespace AnimalAllies.Domain.Models;

public class AnimalType: Entity
{
    private List<Species> _speciesList = [];
    
    private AnimalType(int id, string name, List<Species> specieses) : base(id)
    {
        Name = name;
        AddSpecieses(specieses);
    }
    
    public string Name { get; }
    public IReadOnlyList<Species> SpeciesList => _speciesList;

    public void AddSpecieses(List<Species> speciesList) => _speciesList.AddRange(speciesList);
    

    public static Result<AnimalType> Create(int id, string name, List<Species> speciesList = null)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constraints.Constraints.MAX_ANIMAL_NAME_LENGTH)
        {
            return Result.Failure<AnimalType>($"{name} cannot be null or have length more than {Constraints.Constraints.MAX_ANIMAL_NAME_LENGTH}");
        }

        var animalType = new AnimalType(id, name, speciesList == null ? speciesList : new List<Species>());

        return Result.Success<AnimalType>(animalType);
    }
    
}