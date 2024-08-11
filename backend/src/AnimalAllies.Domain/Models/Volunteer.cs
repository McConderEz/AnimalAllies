using System.Reflection;
using AnimalAllies.Domain.ValueObjects;
using CSharpFunctionalExtensions;

namespace AnimalAllies.Domain.Models;

public class Volunteer: Entity
{
    private List<Requisite> _requisites = [];
    private List<Pet> _pets = [];
    private List<SocialNetwork> _socialNetworks = [];
    
    private Volunteer(){}
    
    private Volunteer(
        FullName fullName,
        string description,
        int workExperience,
        int petsNeedsHelp,
        int petsSearchingHome,
        int petsFoundHome,
        PhoneNumber phone,
        List<SocialNetwork> socialNetworks,
        List<Requisite> requisites,
        List<Pet> pets)
    {
        FullName = fullName;
        Description = description;
        WorkExperience = workExperience;
        PetsNeedsHelp = petsNeedsHelp;
        PetsSearchingHome = petsSearchingHome;
        PetsFoundHome = petsFoundHome;
        Phone = phone;
        AddSocialNetworks(socialNetworks);
        AddRequisites(requisites);
        AddPets(pets);
    }
    
    public FullName FullName { get; private set;}
    public string Description { get; private set; } 
    public int WorkExperience { get; private set; }
    public int PetsNeedsHelp { get; private set; }
    public int PetsSearchingHome { get; private set; }
    public int PetsFoundHome { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public List<SocialNetwork> SocialNetworks => _socialNetworks;

    public IReadOnlyList<Requisite> Requisites => _requisites;
    public IReadOnlyList<Pet> Pets => _pets;

    public void AddRequisites(List<Requisite> requisites) => _requisites.AddRange(requisites);
    public void AddPets(List<Pet> pets) => _pets.AddRange(pets);
    public void AddSocialNetworks(List<SocialNetwork> socialNetworks) => _socialNetworks.AddRange(socialNetworks);

    public static Result<Volunteer> Create(
        string firstName,
        string secondName,
        string patronymic,
        string description,
        int workExperience,
        int petsNeedHelp,
        int petsSearchingHome,
        int petsFoundHome,
        string phoneNumber,
        List<SocialNetwork> socialNetworks,
        List<Requisite> requisites,
        List<Pet> pets)
    {
        if (string.IsNullOrWhiteSpace(description) ||
            description.Length < Constraints.Constraints.MAX_DESCRIPTION_LENGTH)
        {
            return Result.Failure<Volunteer>(
                $"{description} cannot be null or have length more than {Constraints.Constraints.MAX_DESCRIPTION_LENGTH}");
        }
        
        if (workExperience < Constraints.Constraints.MIN_VALUE)
        {
            return Result.Failure<Volunteer>($"{workExperience} cannot be less than {Constraints.Constraints.MIN_VALUE}");
        }
        
        if (petsNeedHelp < Constraints.Constraints.MIN_VALUE)
        {
            return Result.Failure<Volunteer>($"{petsNeedHelp} cannot be less than {Constraints.Constraints.MIN_VALUE}");
        }

        if (petsSearchingHome < Constraints.Constraints.MIN_VALUE)
        {
            return Result.Failure<Volunteer>($"{petsSearchingHome} cannot be less than {Constraints.Constraints.MIN_VALUE}");
        }
        
        if (petsFoundHome < Constraints.Constraints.MIN_VALUE)
        {
            return Result.Failure<Volunteer>($"{petsFoundHome} cannot be less than {Constraints.Constraints.MIN_VALUE}");
        }

        var fullName = FullName.Create(firstName, secondName, patronymic);

        if (fullName.IsFailure)
        {
            return Result.Failure<Volunteer>(fullName.Error);
        }

        var phone = PhoneNumber.Create(phoneNumber);

        if (phone.IsFailure)
        {
            return Result.Failure<Volunteer>(phone.Error);
        }

        var volunteer = new Volunteer(
            fullName.Value,
            description,
            workExperience,
            petsNeedHelp,
            petsSearchingHome,
            petsFoundHome,
            phone.Value,
            socialNetworks ?? [],
            requisites ?? [],
            pets ?? []);

        return Result.Success(volunteer);
    }

}