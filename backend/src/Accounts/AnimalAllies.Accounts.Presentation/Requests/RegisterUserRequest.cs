using AnimalAllies.Accounts.Application.AccountManagement.Commands.Register;

namespace AnimalAllies.Accounts.Presentation.Requests;

public record RegisterUserRequest(string Email, string UserName, string Password)
{
    public RegisterUserCommand ToCommand()
        => new(Email, UserName, Password);
}