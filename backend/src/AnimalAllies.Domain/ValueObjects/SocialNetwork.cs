
using AnimalAllies.Domain.Models;

namespace AnimalAllies.Domain.ValueObjects;

public class SocialNetwork: ValueObject
{
    public string Title { get; }
    public string Url { get; }

    private SocialNetwork(){}
    
    private SocialNetwork(string title, string url)
    {
        Title = title;
        Url = url;
    }
    
    
    public static Result<SocialNetwork> Create(string title, string url)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length > Constraints.Constraints.MAX_VALUE_LENGTH)
        {
            return Result<SocialNetwork>.Failure(new Error("Invalid input",
                $"{title} cannot be null or have length more than {Constraints.Constraints.MAX_VALUE_LENGTH}"));
        }

        if (string.IsNullOrWhiteSpace(url) || url.Length > Constraints.Constraints.MAX_URL_LENGTH)
        {
            return Result<SocialNetwork>.Failure(new Error("Invalid input",
                $"{url} cannot be null or have length more than {Constraints.Constraints.MAX_URL_LENGTH}"));
        }

        return Result<SocialNetwork>.Success(new SocialNetwork(title, url));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Url;
    }
}