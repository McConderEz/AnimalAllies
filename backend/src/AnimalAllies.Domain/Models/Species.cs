using AnimalAllies.Domain.Constraints;
using CSharpFunctionalExtensions;

namespace AnimalAllies.Domain.Models;

public class Species: Entity
{
    private Species(int id, string name, int animalTypeId, AnimalType animalType) : base(id)
    {
        Name = name;
        AnimalTypeId = animalTypeId;
        AnimalType = AnimalType;
    }

    public string Name { get; } = string.Empty;
    
    public int AnimalTypeId { get; }
    public virtual AnimalType? AnimalType { get; }

    public static Result<Species> Create(int id, string name, int animalTypeId, AnimalType animalType = null)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constraints.Constraints.MAX_SPECIES_NAME_LENGTH)
        {
            return Result.Failure<Species>(
                $"{name} cannot be null or have length more than {Constraints.Constraints.MAX_SPECIES_NAME_LENGTH}");
        }

        var species = new Species(id, name, animalTypeId, animalType);

        return Result.Success<Species>(species);
    }
}