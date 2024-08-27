using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer;

public class Volunteer: Entity<VolunteerId>, ISoftDeletable
{
    private readonly List<Pet.Pet> _pets = [];
    private bool _isDeleted = false;
    private Volunteer(VolunteerId id) : base(id){}
    
    public Volunteer(
        VolunteerId volunteerId,
        FullName fullName,
        Email email,
        VolunteerDescription volunteerDescription,
        WorkExperience workExperience,
        PhoneNumber phone,
        VolunteerSocialNetworks socialNetworks,
        VolunteerRequisites requisites)
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
    public VolunteerRequisites Requisites { get; private set; }
    public VolunteerSocialNetworks SocialNetworks { get; private set; }
    public IReadOnlyList<Pet.Pet> Pets => _pets;
    
    public void AddPets(List<Pet.Pet> pets) => _pets.AddRange(pets);

    public int PetsNeedsHelp() => _pets.Count(x => x.HelpStatus == HelpStatus.NeedsHelp);
    public int PetsSearchingHome() => _pets.Count(x => x.HelpStatus == HelpStatus.SearchingHome);
    public int PetsFoundHome() => _pets.Count(x => x.HelpStatus == HelpStatus.FoundHome);
    
    public void SetIsDelete() => _isDeleted = !_isDeleted;
    
    public Result UpdateSocialNetworks(VolunteerSocialNetworks socialNetworks)
    {
        SocialNetworks = socialNetworks;
        return Result.Success();
    }
    public Result UpdateRequisites(VolunteerRequisites requisites)
    {
        Requisites = requisites;
        return Result.Success();
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
}