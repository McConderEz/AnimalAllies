namespace AnimalAllies.Accounts.Application.Models;

public record JwtTokenResult(string AccessToken, Guid Jti);