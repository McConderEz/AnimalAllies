using AnimalAllies.Domain.Models.Common;
using AnimalAllies.Domain.Models.Species;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer.Pet;

public class Pet : Entity<PetId>, ISoftDeletable
{
    private bool _isDeleted = false;
    
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
        AnimalType animalType)
        : base(petId)
    {
        Name = name;
        PetPhysicCharacteristics = petPhysicCharacteristics;
        PetDetails = petDetails;
        Address = address;
        PhoneNumber= phoneNumber;
        HelpStatus = helpStatus;
        AnimalType = animalType;
    }

    public Name Name { get; private set; }
    public PetPhysicCharacteristics PetPhysicCharacteristics { get; private set; }
    public PetDetails PetDetails { get; private set; }
    public Address Address { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public HelpStatus HelpStatus { get; private set; }
    public AnimalType AnimalType { get; private set; }
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

    public Result UpdateAnimalType(AnimalType animalType)
    {
        AnimalType = animalType;
        return Result.Success();
    }

    public void SetIsDelete() => _isDeleted = !_isDeleted;
}