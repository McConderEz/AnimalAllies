using AnimalAllies.Domain.ValueObjects;

namespace AnimalAllies.Domain.Models;

public class Pet : Entity<PetId>
{
    private readonly List<Requisite> _requisites = [];
    private readonly List<PetPhoto> _petPhotos = [];

    private Pet(PetId id) : base(id)
    {
    }

    private Pet(
        PetId petId,
        Name name,
        PetDetails petDetails,
        bool isCastrated,
        DateOnly birthDate,
        bool isVaccinated,
        Address address,
        PhoneNumber phone,
        HelpStatus status,
        SpeciesId speciesID,
        string breedName,
        List<Requisite> requisites,
        List<PetPhoto> petPhotos)
        : base(petId)
    {
        Name = name;
        PetDetails = petDetails;
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

    public Name Name { get; private set; }
    public PetDetails PetDetails { get; private set; }
    public bool IsCastrated { get; private set; } = false;
    public DateOnly BirthDate { get; private set; }
    public bool IsVaccinated { get; private set; } = false;
    public Address Address { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public HelpStatus HelpStatus { get; private set; }
    public SpeciesId SpeciesID { get; private set; }
    public string BreedName { get; private set; }
    public DateTime CreationTime { get; } = DateTime.Now;


    public IReadOnlyList<Requisite> Requisites => _requisites;
    public IReadOnlyList<PetPhoto> PetPhotos => _petPhotos;

    public void AddRequisites(List<Requisite> requisites) => _requisites.AddRange(requisites);
    public void AddPetPhotos(List<PetPhoto> petPhotos) => _petPhotos.AddRange(petPhotos);


    public void SetVaccinated() => IsVaccinated = !IsVaccinated;
    public void SetCastrated() => IsCastrated = !IsCastrated;

    public Result UpdateAddress(Address address)
    {
        this.Address = address;
        return Result.Success();
    }

    public Result UpdatePhoneNumber(PhoneNumber phoneNumber)
    {
        this.Phone = phoneNumber;
        return Result.Success();
    }

    public Result UpdateHelpStatus(HelpStatus helpStatus)
    {
        this.HelpStatus = helpStatus;
        return Result.Success();
    }

    public Result UpdateName(Name name)
    {
        Name = name;
        return Result.Success();
    }

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
        SpeciesId speciesID,
        string breedName,
        List<Requisite> requisites,
        List<PetPhoto> petPhotos)
    {
        
        if (birthDate > DateOnly.FromDateTime(DateTime.Now))
        {
            return Result<Pet>.Failure(Errors.General.ValueIsInvalid(nameof(birthDate)));
        }

        if (string.IsNullOrWhiteSpace(breedName) || breedName.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
        {
            return Result<Pet>.Failure(Errors.General.ValueIsInvalid(nameof(breedName)));
        }

        var nameVo = Name.Create(name);

        if (nameVo.IsFailure)
        {
            return Result<Pet>.Failure(nameVo.Error);
        }
        
        var address = ValueObjects.Address.Create(city, district, houseNumber, flatNumber);
        var phoneNumber = PhoneNumber.Create(phone);
        var helpStatus = ValueObjects.HelpStatus.Create(status);
        var petDetails = PetDetails.Create(description, color, healthInformation, weight, height);

        if (petDetails.IsFailure)
        {
            return Result<Pet>.Failure(petDetails.Error);
        }
        
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
            nameVo.Value,
            petDetails.Value,
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