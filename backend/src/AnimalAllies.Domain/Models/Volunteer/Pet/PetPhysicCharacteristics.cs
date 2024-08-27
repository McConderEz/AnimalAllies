using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer.Pet;

public class PetPhysicCharacteristics : ValueObject
{
    public string Color { get; }
    public string HealthInformation { get; } 
    public double Weight { get; }
    public double Height { get; }
    public bool IsCastrated { get; }
    public bool IsVaccinated { get; }
    
    private PetPhysicCharacteristics(){}

    private PetPhysicCharacteristics(
        string color,
        string healthInformation,
        double weight,
        double height,
        bool isCastrated,
        bool isVaccinated)
    {
        
    }

    public static Result<PetPhysicCharacteristics> Create(
        string color,
        string healthInformation,
        double weight,
        double height,
        bool isCastrated,
        bool isVaccinated)
    {
        
        if (string.IsNullOrWhiteSpace(color) || color.Length > Constraints.Constraints.MAX_PET_COLOR_LENGTH)
        {
            return Errors.General.ValueIsRequired(color);
        }
        
        if (string.IsNullOrWhiteSpace(healthInformation) || healthInformation.Length > Constraints.Constraints.MAX_PET_COLOR_LENGTH)
        {
            return Errors.General.ValueIsRequired(healthInformation);
        }
        
        if (weight > Constraints.Constraints.MIN_VALUE)
        {
            return Errors.General.ValueIsInvalid(nameof(weight));
        }
        
        if (height > Constraints.Constraints.MIN_VALUE)
        {
            return Errors.General.ValueIsRequired(nameof(height));
        }

        return (new PetPhysicCharacteristics(color, healthInformation, weight, height, isCastrated, isVaccinated));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Color;
        yield return HealthInformation;
        yield return Weight;
        yield return Height;
        yield return IsCastrated;
        yield return IsVaccinated;
    }
}