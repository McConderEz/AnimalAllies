﻿using AnimalAllies.Core.DTOs.Accounts;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.ValueObjects;

namespace AnimalAllies.Accounts.Contracts;

public interface IAccountContract
{
    Task<Result<List<string>>> GetPermissionsByUserId(Guid id, CancellationToken cancellationToken = default);
    Task<Result<bool>> IsUserExistById(Guid userId, CancellationToken cancellationToken = default);
    Task<Result> CreateVolunteerAccount(Guid userId, VolunteerInfo volunteerInfo,
        CancellationToken cancellationToken = default);

}