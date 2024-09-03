using System.Text.Json.Serialization;
using AnimalAllies.Domain.Models.Common;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer.Pet;

public class PetRequisites : ValueObject
{
    [JsonPropertyName("Requisites")]
    public IReadOnlyList<Requisite> Requisites { get; }
    
    private PetRequisites(){}

    [JsonConstructor]
    public PetRequisites(IReadOnlyList<Requisite> requisites)
    {
        Requisites = requisites;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Requisites;
    }
}