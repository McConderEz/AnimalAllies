using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using Discussion.Application.Repository;
using Discussion.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Discussion.Infrastructure.Repository;

public class DiscussionRepository: IDiscussionRepository
{
    private readonly WriteDbContext _context;

    public DiscussionRepository(WriteDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DiscussionId>> Create(Domain.Aggregate.Discussion entity, CancellationToken cancellationToken = default)
    {
        await _context.Discussions.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public async Task<Result<Domain.Aggregate.Discussion>> GetById(VolunteerRequestId id, CancellationToken cancellationToken = default)
    {
        var discussion = await _context.Discussions.Include(d => d.Messages)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        if (discussion == null)
            return Errors.General.NotFound();

        return discussion;
    }

    public async Task<Result<Domain.Aggregate.Discussion>> GetByRelationId(Guid relationId, CancellationToken cancellationToken = default)
    {
        var discussion = await _context.Discussions.Include(d => d.Messages)
            .FirstOrDefaultAsync(v => v.RelationId == relationId, cancellationToken);

        if (discussion == null)
            return Errors.General.NotFound();

        return discussion;
    }

    public Result<DiscussionId> Delete(Domain.Aggregate.Discussion entity)
    {
        _context.Discussions.Remove(entity);
        return entity.Id;
    }
}