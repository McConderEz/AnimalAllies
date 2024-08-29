using System.Text.Json.Serialization;
using AnimalAllies.Domain.Models.Common;

namespace AnimalAllies.Domain.Models.Volunteer;

public class VolunteerSocialNetworks : ValueObject
{
    [JsonPropertyName("SocialNetworks")]
    public IReadOnlyList<SocialNetwork> SocialNetworks { get; }

    
    private VolunteerSocialNetworks(){}

    [JsonConstructor]
    public VolunteerSocialNetworks(IReadOnlyList<SocialNetwork> socialNetworks)
    {
        SocialNetworks = socialNetworks;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SocialNetworks;
    }
}