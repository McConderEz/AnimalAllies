namespace AnimalAllies.Core.Options;

public class JwtOptions
{
    public static string JWT = nameof(JWT);
    
    public string Audience { get; init; }
    public string Issuer { get; init; }
    public string Key { get; init; }
    public string ExpiredMinutesTime { get; init; }
}