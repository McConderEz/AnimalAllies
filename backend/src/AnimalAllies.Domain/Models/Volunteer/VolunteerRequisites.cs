using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.ValueObjects;

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