using AnimalAllies.Domain.Models;

namespace AnimalAllies.Domain.ValueObjects;

public class VolunteerDetails : ValueObject
{
 
    private readonly List<Requisite> _requisites = [];
    private readonly List<SocialNetwork> _socialNetworks = [];
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    public IReadOnlyList<Requisite> Requisites => _requisites;
    
    private VolunteerDetails(){}

    private VolunteerDetails(List<Requisite> requisites, List<SocialNetwork> socialNetworks)
    {
        AddRequisites(requisites);
        AddSocialNetworks(socialNetworks);
    }
    
    public void AddRequisites(List<Requisite> requisites) => _requisites.AddRange(requisites);
    public void AddSocialNetworks(List<SocialNetwork> socialNetworks) => _socialNetworks.AddRange(socialNetworks);
    
    public static Result<VolunteerDetails> Create(List<Requisite> requisites,List<SocialNetwork> socialNetworks)
    {
        return Result<VolunteerDetails>.Success(new VolunteerDetails(requisites ?? [], socialNetworks ?? []));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SocialNetworks;
        yield return Requisites;
    }
}