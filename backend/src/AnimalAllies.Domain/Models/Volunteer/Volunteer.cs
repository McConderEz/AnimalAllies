using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer;

public class Volunteer: Entity<VolunteerId>, ISoftDeletable
{
    private bool _isDeleted = false;
    private readonly List<Pet.Pet> _pets = [];
    private List<Requisite> _requisites = [];
    private List<SocialNetwork> _socialNetworks = [];

    private Volunteer(VolunteerId id) : base(id){}
    
    public Volunteer(
        VolunteerId volunteerId,
        FullName fullName,
        Email email,
        VolunteerDescription volunteerDescription,
        WorkExperience workExperience,
        PhoneNumber phone,
        ValueObjectList<SocialNetwork> socialNetworks,
        ValueObjectList<Requisite> requisites)
    : base(volunteerId)
    {
        FullName = fullName;
        Email = email;
        Description = volunteerDescription;
        WorkExperience = workExperience;
        Phone = phone;
        _socialNetworks = socialNetworks;
        _requisites = requisites;       
    }
    
    public FullName FullName { get; private set;}
    public Email Email { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public VolunteerDescription Description { get; private set; }
    public WorkExperience WorkExperience { get; private set; }
    public IReadOnlyList<Requisite> Requisites => _requisites;
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    public IReadOnlyList<Pet.Pet> Pets => _pets;

    public Result AddPet(Pet.Pet pet)
    {

        var position = _pets.Count == 0
            ? Position.First
            : Position.Create(_pets.Count + 1);
        
        if (position.IsFailure)
            return position.Errors;
        
        pet.SetPosition(position.Value);
        _pets.Add(pet);
        return Result.Success();
    }

    public Result UpdatePet(
        PetId petId,
        Name? name,
        PetPhysicCharacteristics? petPhysicCharacteristics,
        PetDetails? petDetails,
        Address? address,
        PhoneNumber? phoneNumber,
        HelpStatus? helpStatus,
        AnimalType? animalType,
        ValueObjectList<Requisite>? requisites)
    {
        var pet = GetPetById(petId);
        if (pet.IsFailure)
            return Errors.General.NotFound();

        pet.Value.UpdatePet(
            name,
            petPhysicCharacteristics,
            petDetails,
            address,
            phoneNumber,
            helpStatus,
            animalType,
            requisites);
        
        return Result.Success();
    }

    public Result UpdatePetStatus(PetId petId, HelpStatus helpStatus)
    {
        var pet = GetPetById(petId);
        if (pet.IsFailure)
            return pet.Errors;

        pet.Value.UpdateHelpStatus(helpStatus);
        return Result.Success();
    }
    
    public Result DeletePetSoft(PetId petId)
    {
        var pet = GetPetById(petId);
        if (pet.IsFailure)
            return pet.Errors;

        pet.Value.Delete();
        return Result.Success();
    }
    
    public Result DeletePetForce(PetId petId)
    {
        var pet = GetPetById(petId);
        if (pet.IsFailure)
            return pet.Errors;

        _pets.Remove(pet.Value);
        pet.Value.Delete();
        return Result.Success();
    }

    public Result MovePet(Pet.Pet pet, Position newPosition)
    {
        var currentPosition = pet.Position;
        
        if(currentPosition == newPosition || _pets.Count == 1)
            return Result.Success();

        var adjustedPosition = IfNewPositionOutOfRange(newPosition);
        if (adjustedPosition.IsFailure)
            return adjustedPosition.Errors;

        newPosition = adjustedPosition.Value;

        var moveResult = MovePetBetweenPositions(newPosition, currentPosition);
        if (moveResult.IsFailure)
            return moveResult.Errors;

        pet.Move(newPosition);
        return Result.Success();
    }

    private Result MovePetBetweenPositions(Position newPosition, Position currentPosition)
    {
        if (newPosition.Value < currentPosition.Value)
        {
            var petsToMove = _pets.Where(p => p.Position.Value >= newPosition.Value
                                              && p.Position.Value < currentPosition.Value);

            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MoveForward();
                if (result.IsFailure)
                {
                    return result.Errors;
                }
            }
        }
        else if (newPosition.Value > currentPosition.Value)
        {
            var petsToMove = _pets.Where(p => p.Position.Value <= newPosition.Value
                                              && p.Position.Value > currentPosition.Value);

            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MoveBack();
                if (result.IsFailure)
                {
                    return result.Errors;
                }
            }
        }

        return Result.Success();
    }

    private Result<Position> IfNewPositionOutOfRange(Position newPosition)
    {
        if (newPosition.Value > _pets.Count)
            return Errors.Volunteer.PetPositionOutOfRange();
        
        return newPosition;
    }

    public int PetsNeedsHelp() => _pets.Count(x => x.HelpStatus == HelpStatus.NeedsHelp);
    public int PetsSearchingHome() => _pets.Count(x => x.HelpStatus == HelpStatus.SearchingHome);
    public int PetsFoundHome() => _pets.Count(x => x.HelpStatus == HelpStatus.FoundHome);
    
    public Result UpdateSocialNetworks(List<SocialNetwork> socialNetworks)
    {
        _socialNetworks = socialNetworks;
        return Result.Success();
    }
    public Result UpdateRequisites(List<Requisite> requisites)
    {
        _requisites = requisites;
        return Result.Success();
    }

    public  Result<Pet.Pet> GetPetById(PetId id)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == id);

        if (pet == null)
            return Errors.General.NotFound(id.Id);

        return pet;
    }

    public Result UpdateInfo(
        FullName? fullName,
        Email? email,
        PhoneNumber? phoneNumber,
        VolunteerDescription? description,
        WorkExperience? workExperience)
    {
        FullName = fullName ?? FullName;
        Email = email ?? Email;
        Phone = phoneNumber ?? Phone;
        Description = description ?? Description;
        WorkExperience = workExperience ?? WorkExperience;

        return Result.Success();
    }

    public void Delete() => _isDeleted = !_isDeleted;
}