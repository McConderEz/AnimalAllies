using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.ValueObjects;

namespace AnimalAllies.Domain.ValueObjects;

public class PetDetails : ValueObject
{
    public string Description { get; }
    public string Color { get; }
    public string HealthInformation { get; } 
    public double Weight { get; }
    public double Height { get; }

    private PetDetails(){}

    private PetDetails(
        string description,
        string color,
        string healthInformation,
        double weight,
        double height)
    {
        
    }

    public static Result<PetDetails> Create(
        string description,
        string color,
        string healthInformation,
        double weight,
        double height)
    {
        if (string.IsNullOrWhiteSpace(description) || description.Length > Constraints.Constraints.MAX_DESCRIPTION_LENGTH)
        {
            return Result<PetDetails>.Failure(new Error("Invalid input",
                $"{description} cannot be null or have length more than {Constraints.Constraints.MAX_DESCRIPTION_LENGTH}"));
        }
        
        if (string.IsNullOrWhiteSpace(color) || color.Length > Constraints.Constraints.MAX_PET_COLOR_LENGTH)
        {
            return Result<PetDetails>.Failure(new Error("Invalid input",
                $"{color} cannot be null or have length more than {Constraints.Constraints.MAX_PET_COLOR_LENGTH}"));
        }
        
        if (string.IsNullOrWhiteSpace(healthInformation) || healthInformation.Length > Constraints.Constraints.MAX_PET_COLOR_LENGTH)
        {
            return Result<PetDetails>.Failure(new Error("Invalid input",
                $"{healthInformation} cannot be null or have length more than {Constraints.Constraints.MAX_PET_INFORMATION_LENGTH}"));
        }
        
        if (weight > Constraints.Constraints.MIN_VALUE)
        {
            return Result<PetDetails>.Failure(new Error("Invalid input",
                $"{weight} must be more than {Constraints.Constraints.MIN_VALUE}"));
        }
        
        if (height > Constraints.Constraints.MIN_VALUE)
        {
            return Result<PetDetails>.Failure(new Error("Invalid input",
                $"{height} must be more than {Constraints.Constraints.MIN_VALUE}"));
        }

        return Result<PetDetails>.Success(new PetDetails(description, color, healthInformation, weight, height));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
        yield return Color;
        yield return HealthInformation;
        yield return Weight;
        yield return Height;
    }
}