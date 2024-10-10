using AnimalAllies.Accounts.Application.AccountManagement.Commands.Login;

namespace AnimalAllies.Accounts.Presentation.Requests;

public record LoginUserRequest(string Email, string Password)
{
    public LoginUserCommand ToCommand()
        => new(Email, Password);
}