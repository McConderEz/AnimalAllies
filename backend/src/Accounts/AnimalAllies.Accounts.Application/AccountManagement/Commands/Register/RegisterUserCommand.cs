using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.DTOs.ValueObjects;
using AnimalAllies.SharedKernel.Shared.ValueObjects;

namespace AnimalAllies.Accounts.Application.AccountManagement.Commands.Register;

public record RegisterUserCommand(
    string Email,
    string UserName,
    FullNameDto FullNameDto,
    IEnumerable<SocialNetworkDto>? SocialNetworkDtos,
    string Password): ICommand;