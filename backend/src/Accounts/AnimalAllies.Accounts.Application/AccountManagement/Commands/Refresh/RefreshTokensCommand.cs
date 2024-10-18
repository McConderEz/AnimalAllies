using AnimalAllies.Core.Abstractions;

namespace AnimalAllies.Accounts.Application.AccountManagement.Commands.Refresh;

public record RefreshTokensCommand(string AccessToken, Guid RefreshToken) : ICommand;