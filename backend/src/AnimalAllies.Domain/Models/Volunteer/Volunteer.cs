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
        SocialNetworks = socialNetworks;
        Requisites = requisites;
    }
    
    public FullName FullName { get; private set;}
    public Email Email { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public VolunteerDescription Description { get; private set; }
    public WorkExperience WorkExperience { get; private set; }
    public ValueObjectList<Requisite> Requisites { get; private set; }
    public ValueObjectList<SocialNetwork> SocialNetworks { get; private set; }
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

    public int PetsNeedsHelp() => _pets.Count(x => x.HelpStatus == HelpStatus.NeedsHelp);
    public int PetsSearchingHome() => _pets.Count(x => x.HelpStatus == HelpStatus.SearchingHome);
    public int PetsFoundHome() => _pets.Count(x => x.HelpStatus == HelpStatus.FoundHome);
    
    public Result UpdateSocialNetworks(ValueObjectList<SocialNetwork> socialNetworks)
    {
        SocialNetworks = socialNetworks;
        return Result.Success();
    }
    public Result UpdateRequisites(ValueObjectList<Requisite> requisites)
    {
        Requisites = requisites;
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