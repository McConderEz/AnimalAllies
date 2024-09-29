using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Models.Species;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer.Pet;

public class Pet : Entity<PetId>, ISoftDeletable
{
    private bool _isDeleted = false;
    
    private Pet(PetId id) : base(id)
    {
    }

    public Pet(
        PetId petId,
        Name name,
        PetPhysicCharacteristics petPhysicCharacteristics,
        PetDetails petDetails,
        Address address,
        PhoneNumber phoneNumber,
        HelpStatus helpStatus,
        AnimalType animalType,
        ValueObjectList<Requisite> requisites,
        ValueObjectList<PetPhoto>? petPhotoDetails)
        : base(petId)
    {
        Name = name;
        PetPhysicCharacteristics = petPhysicCharacteristics;
        PetDetails = petDetails;
        Address = address;
        PhoneNumber= phoneNumber;
        HelpStatus = helpStatus;
        AnimalType = animalType;
        Requisites = requisites;
        PetPhotoDetails = petPhotoDetails;
    }

    public Name Name { get; private set; }
    public PetPhysicCharacteristics PetPhysicCharacteristics { get; private set; }
    public PetDetails PetDetails { get; private set; }
    public Address Address { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public HelpStatus HelpStatus { get; private set; }
    public AnimalType AnimalType { get; private set; }
    public Position Position { get; private set; }
    public ValueObjectList<Requisite> Requisites { get; private set; }
    public ValueObjectList<PetPhoto>? PetPhotoDetails { get; private set; }

    public Result AddPhotos(ValueObjectList<PetPhoto>? photos)
    {
        PetPhotoDetails = photos;

        return Result.Success();
    }

    public Result DeletePhotos()
    {
        PetPhotoDetails = new ValueObjectList<PetPhoto>([]);
        
        return Result.Success();
    }

    public void SetPosition(Position position)
    {
        Position = position;
    }
    
    public void Delete() => _isDeleted = !_isDeleted;

    public Result MoveForward()
    {
        var newPosition = Position.Forward();
        if (newPosition.IsFailure)
            return newPosition.Errors;

        Position = newPosition.Value;

        return Result.Success();
    }
    
    public Result MoveBack()
    {
        var newPosition = Position.Back();
        if (newPosition.IsFailure)
            return newPosition.Errors;

        Position = newPosition.Value;

        return Result.Success();
    }

    public Result UpdateHelpStatus(HelpStatus helpStatus)
    {
        HelpStatus = helpStatus;
        
        return Result.Success();
    }

    public void Move(Position newPosition)
    {
        Position = newPosition;
    }

    public void UpdatePet(
        Name? name,
        PetPhysicCharacteristics? petPhysicCharacteristics,
        PetDetails? petDetails,
        Address? address,
        PhoneNumber? phoneNumber,
        HelpStatus? helpStatus,
        AnimalType? animalType,
        ValueObjectList<Requisite>? requisites)
    {
        Name = name ?? Name;
        PetPhysicCharacteristics = petPhysicCharacteristics ?? PetPhysicCharacteristics;
        PetDetails = petDetails ?? PetDetails;
        Address = address ?? Address;
        PhoneNumber = phoneNumber ?? PhoneNumber;
        HelpStatus = helpStatus ?? HelpStatus;
        AnimalType = animalType ?? AnimalType;
        Requisites = requisites ?? Requisites;
    }
}