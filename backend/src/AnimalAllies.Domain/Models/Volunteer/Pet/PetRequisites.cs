using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer.Pet;

public class PetRequisites : ValueObject
{
    public IReadOnlyList<Requisite> Requisites { get; }
    
    private PetRequisites(){}

    private PetRequisites(IEnumerable<Requisite> requisites)
    {
        Requisites = requisites.ToList();
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Requisites;
    }
}