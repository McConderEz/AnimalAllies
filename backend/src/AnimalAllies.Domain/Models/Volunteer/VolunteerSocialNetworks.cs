using AnimalAllies.Domain.Common;

namespace AnimalAllies.Domain.Models.Volunteer;

public class VolunteerSocialNetworks : ValueObject
{
    public IReadOnlyList<SocialNetwork> SocialNetworks { get; }

    
    private VolunteerSocialNetworks(){}

    public VolunteerSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
    {
        SocialNetworks = socialNetworks.ToList();
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SocialNetworks;
    }
}