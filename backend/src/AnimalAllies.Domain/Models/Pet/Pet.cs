using AnimalAllies.Domain.Shared;
using AnimalAllies.Domain.ValueObjects;

namespace AnimalAllies.Domain.Models.Pet;

public class Pet : Entity<PetId>
{

    private Pet(PetId id) : base(id)
    {
    }

    private Pet(
        PetId petId,
        Name name,
        PetPhysicCharacteristics petPhysicCharacteristics,
        PetDetails petDetails,
        Address address,
        PhoneNumber phoneNumber,
        HelpStatus helpStatus,
        SpeciesId speciesId,
        string breedName)
        : base(petId)
    {
        Name = name;
        PetPhysicCharacteristics = petPhysicCharacteristics;
        PetDetails = petDetails;
        Address = address;
        PhoneNumber= phoneNumber;
        HelpStatus = helpStatus;
        SpeciesId = speciesId;
        BreedName = breedName;
    }

    public Name Name { get; private set; }
    public PetPhysicCharacteristics PetPhysicCharacteristics { get; private set; }
    public PetDetails PetDetails { get; private set; }
    public Address Address { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public HelpStatus HelpStatus { get; private set; }
    public SpeciesId SpeciesId { get; private set; }
    public string BreedName { get; private set; }
    public PetRequisites Requisites { get; private set; }
    public PetPhotoDetails PetPhotoDetails { get; private set; }
    
    

    public Result UpdateAddress(Address address)
    {
        Address = address;
        return Result.Success();
    }

    public Result UpdatePhoneNumber(PhoneNumber phoneNumber)
    {
        PhoneNumber = phoneNumber;
        return Result.Success();
    }

    public Result UpdateHelpStatus(HelpStatus helpStatus)
    {
        HelpStatus = helpStatus;
        return Result.Success();
    }

    public Result UpdateName(Name name)
    {
        Name = name;
        return Result.Success();
    }

}