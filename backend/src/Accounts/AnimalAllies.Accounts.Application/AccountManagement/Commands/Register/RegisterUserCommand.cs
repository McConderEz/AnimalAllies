using AnimalAllies.Core.Abstractions;

namespace AnimalAllies.Accounts.Application.AccountManagement.Commands.Register;

public record RegisterUserCommand(string Email, string UserName, string Password): ICommand;