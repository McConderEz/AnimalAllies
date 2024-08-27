using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer;

public class VolunteerRequisites : ValueObject
{
    public IReadOnlyList<Requisite> Requisites { get; }
    
    private VolunteerRequisites(){}

    public VolunteerRequisites(IEnumerable<Requisite> requisites)
    {
        Requisites = requisites.ToList();
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Requisites;
    }
}