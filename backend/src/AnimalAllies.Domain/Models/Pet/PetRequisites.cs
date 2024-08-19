using AnimalAllies.Domain.Shared;
using AnimalAllies.Domain.ValueObjects;

namespace AnimalAllies.Domain.Models.Pet;

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