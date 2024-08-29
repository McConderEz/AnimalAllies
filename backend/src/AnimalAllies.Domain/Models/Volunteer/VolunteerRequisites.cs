using System.Text.Json.Serialization;
using AnimalAllies.Domain.Models.Common;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Domain.Models.Volunteer;

public class VolunteerRequisites : ValueObject
{
    [JsonPropertyName("Requisites")]
    public IReadOnlyList<Requisite> Requisites { get; }
    
    private VolunteerRequisites(){}

    [JsonConstructor]
    public VolunteerRequisites(IReadOnlyList<Requisite> requisites)
    {
        Requisites = requisites;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Requisites;
    }
}