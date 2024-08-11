using AnimalAllies.Domain.ValueObjects;
using CSharpFunctionalExtensions;

namespace AnimalAllies.Domain.Models;

public class Pet: Entity
{
    private List<Requisite> _requisites = [];
    private List<PetPhoto> _petPhotos = [];
    private Pet(){}
    private Pet(
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
        Species species,
        AnimalType animalType,
        List<Requisite> requisites,
        List<PetPhoto> petPhotos) 
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
        Species = species;
        AnimalType = animalType;
        AddRequisites(requisites);
        AddPetPhotos(petPhotos);
    }
    
    public string Name { get; private set; } 
    public string Description { get; private set; }
    public string Color { get; private set; }
    public string HealthInformation { get; private set;} 
    public double Weight { get; private set; }
    public double Height { get; private set; }
    public bool IsCastrated { get; private set; } = false;
    public DateOnly BirthDate { get; private set; }
    public bool IsVaccinated { get; private set; } = false;
    public Address Address { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public HelpStatus HelpStatus { get; private set; }
    public Species Species { get; private set; }
    public AnimalType AnimalType { get; private set; }
    public DateTime CreationTime { get; } = DateTime.Now;
    
    
    public IReadOnlyList<Requisite> Requisites => _requisites;
    public IReadOnlyList<PetPhoto> PetPhotos => _petPhotos;

    public void AddRequisites(List<Requisite> requisites) => _requisites.AddRange(requisites);
    public void AddPetPhotos(List<PetPhoto> petPhotos) => _petPhotos.AddRange(petPhotos);

    public void SetVaccinated() => IsVaccinated = !IsVaccinated;
    public void SetCastrated() => IsCastrated = !IsCastrated;
    
    public static Result<Pet> Create(
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
        string speciesValue,
        string animalTypeValue,
        List<Requisite> requisites,
        List<PetPhoto> petPhotos)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
        {
            return Result.Failure<Pet>(
                $"{name} cannot be null or have length more than {Constraints.Constraints.MAX_VALUE_LENGTH}");
        }
        
        if (string.IsNullOrWhiteSpace(description) || description.Length > Constraints.Constraints.MAX_DESCRIPTION_LENGTH)
        {
            return Result.Failure<Pet>(
                $"{description} cannot be null or have length more than {Constraints.Constraints.MAX_DESCRIPTION_LENGTH}");
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
        
        if (weight > Constraints.Constraints.MIN_VALUE)
        {
            return Result.Failure<Pet>(
                $"{weight} must be more than {Constraints.Constraints.MIN_VALUE}");
        }
        
        if (height > Constraints.Constraints.MIN_VALUE)
        {
            return Result.Failure<Pet>(
                $"{height} must be more than {Constraints.Constraints.MIN_VALUE}");
        }

        if (birthDate > DateOnly.FromDateTime(DateTime.Now))
        {
            return Result.Failure<Pet>($"{birthDate} cannot be more than {DateOnly.FromDateTime(DateTime.Now)}");
        }

        var address = ValueObjects.Address.Create(city, district, houseNumber, flatNumber);
        var phoneNumber = PhoneNumber.Create(phone);
        var helpStatus = ValueObjects.HelpStatus.Create(status);
        var species = ValueObjects.Species.Create(speciesValue);
        var animalType = ValueObjects.AnimalType.Create(animalTypeValue);

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

        if (species.IsFailure)
        {
            return Result.Failure<Pet>(species.Error);
        }
        
        if (animalType.IsFailure)
        {
            return Result.Failure<Pet>(animalType.Error);
        }

        var pet = new Pet(
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
            species.Value,
            animalType.Value,
            requisites ?? [],
            petPhotos ?? []);

        return Result.Success(pet);
    }

}