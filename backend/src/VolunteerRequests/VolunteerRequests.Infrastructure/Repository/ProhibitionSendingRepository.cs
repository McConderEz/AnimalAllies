using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using Microsoft.EntityFrameworkCore;
using VolunteerRequests.Application.Repository;
using VolunteerRequests.Domain.Aggregates;
using VolunteerRequests.Infrastructure.DbContexts;

namespace VolunteerRequests.Infrastructure.Repository;

public class ProhibitionSendingRepository(WriteDbContext context) : IProhibitionSendingRepository
{

    public async Task<Result<ProhibitionSendingId>> Create(
        ProhibitionSending entity, CancellationToken cancellationToken = default)
    {
        await context.ProhibitionsSending.AddAsync(entity, cancellationToken);

        return entity.Id;
    }

    public async Task<Result<ProhibitionSending?>> GetById(
        ProhibitionSendingId id, CancellationToken cancellationToken = default)
    {
        return await context.ProhibitionsSending
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Result<ProhibitionSending>> GetByUserId(
        Guid userId, CancellationToken cancellationToken = default)
    {
        var result = await context.ProhibitionsSending
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (result is null)
            return Errors.General.NotFound(userId);

        return result;
    }

    public Result Delete(ProhibitionSending entity)
    {
        context.ProhibitionsSending.Remove(entity);

        return Result.Success();
    }
}