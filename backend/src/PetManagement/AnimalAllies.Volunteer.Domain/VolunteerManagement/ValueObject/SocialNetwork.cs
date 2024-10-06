using System.Text.Json.Serialization;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;


namespace AnimalAllies.Volunteer.Domain.VolunteerManagement.ValueObject;

public class SocialNetwork: SharedKernel.Shared.ValueObject
{
    public string Title { get; }
    public string Url { get; }

    private SocialNetwork(){}
    
    [JsonConstructor]
    private SocialNetwork(string title, string url)
    {
        Title = title;
        Url = url;
    }
    
    
    public static Result<SocialNetwork> Create(string title, string url)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length > Constraints.MAX_VALUE_LENGTH)
        {
            return Errors.General.ValueIsRequired(title);
        }

        if (string.IsNullOrWhiteSpace(url) || url.Length > Constraints.MAX_URL_LENGTH)
        {
            return Errors.General.ValueIsRequired(url);
        }

        return new SocialNetwork(title, url);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Url;
    }
}