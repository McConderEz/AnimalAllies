using AnimalAllies.Domain.ValueObjects;

namespace AnimalAllies.Domain.Models;

public class Pet: Entity<PetId>
{
    private readonly List<Requisite> _requisites = [];
    private readonly List<PetPhoto> _petPhotos = [];
    private Pet(PetId id): base(id) {}
    private Pet(
        PetId petId,
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
        int speciesID,
        string breedName,
        List<Requisite> requisites,
        List<PetPhoto> petPhotos) 
    : base(petId)
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
        SpeciesID = speciesID;
        BreedName = breedName;
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
    public int SpeciesID { get; private set; }
    public string BreedName { get; private set; }
    public DateTime CreationTime { get; } = DateTime.Now;
    
    
    public IReadOnlyList<Requisite> Requisites => _requisites;
    public IReadOnlyList<PetPhoto> PetPhotos => _petPhotos;

    public void AddRequisites(List<Requisite> requisites) => _requisites.AddRange(requisites);
    public void AddPetPhotos(List<PetPhoto> petPhotos) => _petPhotos.AddRange(petPhotos);
    
    public void SetVaccinated() => IsVaccinated = !IsVaccinated;
    public void SetCastrated() => IsCastrated = !IsCastrated;
    
    //TODO: Добавить методы изменения ValueObject`ов
    
    public static Result<Pet> Create(
        PetId petId,
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
        int speciesID,
        string breedName,
        List<Requisite> requisites,
        List<PetPhoto> petPhotos)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
        {
            return Result<Pet>.Failure(new Error("Invalid input",
                $"{name} cannot be null or have length more than {Constraints.Constraints.MAX_VALUE_LENGTH}"));
        }
        
        if (string.IsNullOrWhiteSpace(description) || description.Length > Constraints.Constraints.MAX_DESCRIPTION_LENGTH)
        {
            return Result<Pet>.Failure(new Error("Invalid input",
                $"{description} cannot be null or have length more than {Constraints.Constraints.MAX_DESCRIPTION_LENGTH}"));
        }
        
        if (string.IsNullOrWhiteSpace(color) || color.Length > Constraints.Constraints.MAX_PET_COLOR_LENGTH)
        {
            return Result<Pet>.Failure(new Error("Invalid input",
                $"{color} cannot be null or have length more than {Constraints.Constraints.MAX_PET_COLOR_LENGTH}"));
        }
        
        if (string.IsNullOrWhiteSpace(healthInformation) || healthInformation.Length > Constraints.Constraints.MAX_PET_COLOR_LENGTH)
        {
            return Result<Pet>.Failure(new Error("Invalid input",
                $"{healthInformation} cannot be null or have length more than {Constraints.Constraints.MAX_PET_INFORMATION_LENGTH}"));
        }
        
        if (weight > Constraints.Constraints.MIN_VALUE)
        {
            return Result<Pet>.Failure(new Error("Invalid input",
                $"{weight} must be more than {Constraints.Constraints.MIN_VALUE}"));
        }
        
        if (height > Constraints.Constraints.MIN_VALUE)
        {
            return Result<Pet>.Failure(new Error("Invalid input",
                $"{height} must be more than {Constraints.Constraints.MIN_VALUE}"));
        }

        if (birthDate > DateOnly.FromDateTime(DateTime.Now))
        {
            return Result<Pet>.Failure(new Error("Invalid input",$"{birthDate} cannot be more than {DateOnly.FromDateTime(DateTime.Now)}"));
        }

        if (string.IsNullOrWhiteSpace(breedName) || breedName.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
        {
            return Result<Pet>.Failure(new Error("Invalid input",
                $"{breedName} cannot be null or have length more than {Constraints.Constraints.MAX_VALUE_LENGTH}"));
        }

        var address = ValueObjects.Address.Create(city, district, houseNumber, flatNumber);
        var phoneNumber = PhoneNumber.Create(phone);
        var helpStatus = ValueObjects.HelpStatus.Create(status);

        if (address.IsFailure)
        {
            return Result<Pet>.Failure(address.Error);
        }
        
        if (phoneNumber.IsFailure)
        {
            return Result<Pet>.Failure(phoneNumber.Error);
        }
        
        if (helpStatus.IsFailure)
        {
            return Result<Pet>.Failure(helpStatus.Error);
        }

        var pet = new Pet(
            petId,
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
            speciesID,
            breedName,
            requisites ?? [],
            petPhotos ?? []);

        return Result<Pet>.Success(pet);
    }

}