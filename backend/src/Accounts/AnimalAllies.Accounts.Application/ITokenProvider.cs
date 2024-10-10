using AnimalAllies.Accounts.Domain;

namespace AnimalAllies.Accounts.Application;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
}