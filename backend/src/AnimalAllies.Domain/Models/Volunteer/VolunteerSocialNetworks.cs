using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.ValueObjects;

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