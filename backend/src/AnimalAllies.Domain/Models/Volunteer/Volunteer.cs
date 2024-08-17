using System.Reflection;
using AnimalAllies.Domain.ValueObjects;


namespace AnimalAllies.Domain.Models;

public class Volunteer: Entity<VolunteerId>
{
    private readonly List<Requisite> _requisites = [];
    private readonly List<Pet> _pets = [];
    private readonly List<SocialNetwork> _socialNetworks = [];

    private Volunteer(VolunteerId id) : base(id)
    {
        
    }
    
    private Volunteer(
        VolunteerId volunteerId,
        FullName fullName,
        string description,
        int workExperience,
        PhoneNumber phone,
        List<SocialNetwork> socialNetworks,
        List<Requisite> requisites,
        List<Pet> pets)
    : base(volunteerId)
    {
        FullName = fullName;
        Description = description;
        WorkExperience = workExperience;
        Phone = phone;
        AddSocialNetworks(socialNetworks);
        AddRequisites(requisites);
        AddPets(pets);
    }
    
    public FullName FullName { get; private set;}
    public string Description { get; private set; }
    public int WorkExperience { get; private set; }

    public int PetsNeedsHelp() => _pets.Count(x => x.HelpStatus == HelpStatus.NeedsHelp);
    public int PetsSearchingHome() => _pets.Count(x => x.HelpStatus == HelpStatus.SearchingHome);
    public int PetsFoundHome() => _pets.Count(x => x.HelpStatus == HelpStatus.FoundHome);
    
    public PhoneNumber Phone { get; private set; }
    public List<SocialNetwork> SocialNetworks => _socialNetworks;

    public IReadOnlyList<Requisite> Requisites => _requisites;
    public IReadOnlyList<Pet> Pets => _pets;

    public void AddRequisites(List<Requisite> requisites) => _requisites.AddRange(requisites);
    public void AddPets(List<Pet> pets) => _pets.AddRange(pets);
    public void AddSocialNetworks(List<SocialNetwork> socialNetworks) => _socialNetworks.AddRange(socialNetworks);

    public Result UpdateWorkExperience(int workExperience)
    {
        if (workExperience < 0 || workExperience > Constraints.Constraints.MAX_EXP_VALUE)
        {
            return Result.Failure(new Error("Invalid input",
                $"workExp cannot be less than 0 or more than {Constraints.Constraints.MAX_EXP_VALUE}"));
        }

        WorkExperience = workExperience;
        return Result.Success();
    }

    public Result UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description) ||
            description.Length > Constraints.Constraints.MAX_DESCRIPTION_LENGTH)
        {
            return Result.Failure(new Error("Invalid Input",
                $"{description} cannot be null or have length more than {Constraints.Constraints.MAX_DESCRIPTION_LENGTH}"));
        }

        Description = description;
        return Result.Success();
    }
    
    public Result UpdatePhoneNumber(PhoneNumber phoneNumber)
    {
        this.Phone = phoneNumber;
        return Result.Success();
    }
    
    public Result UpdateFullName(FullName fullName)
    {
        this.FullName = fullName;
        return Result.Success();
    }
    
    public static Result<Volunteer> Create(
        VolunteerId volunteerId,
        string firstName,
        string secondName,
        string patronymic,
        string description,
        int workExperience,
        string phoneNumber,
        List<SocialNetwork>? socialNetworks,
        List<Requisite>? requisites,
        List<Pet>? pets)
    {
        if (string.IsNullOrWhiteSpace(description) ||
            description.Length > Constraints.Constraints.MAX_DESCRIPTION_LENGTH)
        {
            return Result<Volunteer>.Failure(new Error("Invalid Input",
                $"{description} cannot be null or have length more than {Constraints.Constraints.MAX_DESCRIPTION_LENGTH}"));
        }
        
        if (workExperience < Constraints.Constraints.MIN_VALUE)
        {
            return Result<Volunteer>.Failure(new Error("Invalid Input",$"{workExperience} cannot be less than {Constraints.Constraints.MIN_VALUE}"));
        }

        var fullName = FullName.Create(firstName, secondName, patronymic);

        if (fullName.IsFailure)
        {
            return Result<Volunteer>.Failure(fullName.Error);
        }

        var phone = PhoneNumber.Create(phoneNumber);

        if (phone.IsFailure)
        {
            return Result<Volunteer>.Failure(phone.Error);
        }

        var volunteer = new Volunteer(
            volunteerId,
            fullName.Value,
            description,
            workExperience,
            phone.Value,
            socialNetworks ?? [],
            requisites ?? [],
            pets ?? []);

        return Result<Volunteer>.Success(volunteer);
    }

}