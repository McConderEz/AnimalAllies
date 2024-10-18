using AnimalAllies.Core.DTOs.ValueObjects;
using AnimalAllies.SharedKernel.Shared.ValueObjects;

namespace AnimalAllies.Accounts.Contracts.Requests;

public record RegisterUserRequest(
    string Email,
    string UserName,
    FullNameDto FullNameDto,
    string Password);
