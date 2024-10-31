using AnimalAllies.Core.Abstractions;

namespace AnimalAllies.Accounts.Application.AccountManagement.Commands.DeleteBanUser;

public record DeleteBannedUserCommand(Guid UserId) : ICommand;
