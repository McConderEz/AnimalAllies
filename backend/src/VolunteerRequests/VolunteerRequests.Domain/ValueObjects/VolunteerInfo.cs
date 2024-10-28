using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.ValueObjects;

namespace VolunteerRequests.Domain.ValueObjects;

public class VolunteerInfo : ValueObject
{
    public FullName FullName { get; }
    public Email Email { get; }
    public PhoneNumber PhoneNumber { get; }
    public WorkExperience WorkExperience { get; }
    public VolunteerDescription VolunteerDescription { get; }
    public IReadOnlyList<SocialNetwork> SocialNetworks { get; }
    
    private VolunteerInfo(){}

    public VolunteerInfo(
        FullName fullName, 
        Email email, 
        PhoneNumber phoneNumber,
        WorkExperience workExperience,
        VolunteerDescription volunteerDescription,
        IEnumerable<SocialNetwork> socialNetworks)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        WorkExperience = workExperience;
        VolunteerDescription = volunteerDescription;
        SocialNetworks = socialNetworks.ToList();
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FullName;
        yield return Email;
        yield return PhoneNumber;
        yield return WorkExperience;
        yield return VolunteerDescription;
        yield return SocialNetworks;
    }
}