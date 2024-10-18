using AnimalAllies.Accounts.Domain;
using AnimalAllies.SharedKernel.Shared;

namespace AnimalAllies.Accounts.Application.Managers;

public interface IParticipantManager
{
    Task<Result> CreateParticipantAccount(
        ParticipantAccount participantAccount, CancellationToken cancellationToken = default);
}