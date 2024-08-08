using AnimalAllies.Domain.ValueObjects;
using CSharpFunctionalExtensions;

namespace AnimalAllies.Domain.Models;

public class Pet: Entity
{
    private List<Requisite> _requisites = [];
    private Pet(int id,
        string name,
        string description,
        string color,
        string healthInformation,
        double weight,
        double height,
        bool isCastrated,
        DateOnly birthDate,
        bool isVaccinated,
        Address address,
        PhoneNumber phone,
        HelpStatus status,
        int speciesId,
        Species species = null,
        List<Requisite> requisites = null) 
        : base(id)
    {
        Name = name;
        Description = description;
        Color = color;
        HealthInformation = healthInformation;
        Weight = weight;
        Height = height;
        IsCastrated = isCastrated;
        BirthDate = birthDate;
        IsVaccinated = isVaccinated;
        Address = address;
        Phone = phone;
        HelpStatus = status;
        SpeciesId = speciesId;
        Species = species;
        AddRequisite(requisites);
    }
    
    public string Name { get; } = String.Empty;
    public string Description { get; } = string.Empty;
    public string Color { get; } = string.Empty;
    public string HealthInformation { get; } = string.Empty;
    public double Weight { get; }
    public double Height { get; }
    public bool IsCastrated { get; } = false;
    public DateOnly BirthDate { get; }
    public bool IsVaccinated { get; } = false;
    public Address Address { get; }
    public PhoneNumber Phone { get; }
    public HelpStatus HelpStatus { get; }
    public DateTime CreationTime { get; } = DateTime.Now;
    
    public int SpeciesId { get; }
    public virtual Species? Species { get; }
    
    public IReadOnlyList<Requisite> Requisites => _requisites;

    public void AddRequisite(List<Requisite> requisites) => _requisites.AddRange(requisites);

    public static Result<Pet> Create(
        int id,
        string name,
        string description,
        string color,
        string healthInformation,
        double weight,
        double height,
        bool isCastrated,
        DateOnly birthDate,
        bool isVaccinated,
        string city,
        string district,
        int houseNumber,
        int flatNumber,
        string phone,
        string status,
        int speciesId,
        Species species = null,
        List<Requisite> requisites = null)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constraints.Constraints.MAX_PET_NAME_LENGTH)
        {
            return Result.Failure<Pet>(
                $"{name} cannot be null or have length more than {Constraints.Constraints.MAX_PET_NAME_LENGTH}");
        }
        
        if (string.IsNullOrWhiteSpace(description) || description.Length > Constraints.Constraints.MAX_PET_DESCRIPTION_LENGTH)
        {
            return Result.Failure<Pet>(
                $"{description} cannot be null or have length more than {Constraints.Constraints.MAX_PET_DESCRIPTION_LENGTH}");
        }
        
        if (string.IsNullOrWhiteSpace(color) || color.Length > Constraints.Constraints.MAX_PET_COLOR_LENGTH)
        {
            return Result.Failure<Pet>(
                $"{color} cannot be null or have length more than {Constraints.Constraints.MAX_PET_COLOR_LENGTH}");
        }
        
        if (string.IsNullOrWhiteSpace(healthInformation) || healthInformation.Length > Constraints.Constraints.MAX_PET_COLOR_LENGTH)
        {
            return Result.Failure<Pet>(
                $"{healthInformation} cannot be null or have length more than {Constraints.Constraints.MAX_PET_INFORMATION_LENGTH}");
        }
        
        if (weight > Constraints.Constraints.MIN_PET_WEIGHT)
        {
            return Result.Failure<Pet>(
                $"{weight} must be more than {Constraints.Constraints.MIN_PET_WEIGHT}");
        }
        
        if (height > Constraints.Constraints.MIN_PET_HEIGHT)
        {
            return Result.Failure<Pet>(
                $"{height} must be more than {Constraints.Constraints.MIN_PET_HEIGHT}");
        }

        if (birthDate > DateOnly.FromDateTime(DateTime.Now))
        {
            return Result.Failure<Pet>($"{birthDate} cannot be more than {DateOnly.FromDateTime(DateTime.Now)}");
        }

        var address = ValueObjects.Address.Create(city, district, houseNumber, flatNumber);
        var phoneNumber = PhoneNumber.Create(phone);
        var helpStatus = ValueObjects.HelpStatus.Create(status);

        if (address.IsFailure)
        {
            return Result.Failure<Pet>(address.Error);
        }
        
        if (phoneNumber.IsFailure)
        {
            return Result.Failure<Pet>(phoneNumber.Error);
        }
        
        if (helpStatus.IsFailure)
        {
            return Result.Failure<Pet>(helpStatus.Error);
        }

        var pet = new Pet(id,
            name,
            description,
            color,
            healthInformation,
            weight,
            height,
            isCastrated,
            birthDate,
            isVaccinated,
            address.Value,
            phoneNumber.Value,
            helpStatus.Value,
            speciesId,
            species,
            requisites == null ? requisites : new List<Requisite>()
        );

        return Result.Success(pet);
    }

}