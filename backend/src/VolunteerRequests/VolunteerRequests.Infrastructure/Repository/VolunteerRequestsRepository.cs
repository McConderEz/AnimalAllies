using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using Microsoft.EntityFrameworkCore;
using VolunteerRequests.Application.Repository;
using VolunteerRequests.Domain.Aggregate;
using VolunteerRequests.Infrastructure.DbContexts;

namespace VolunteerRequests.Infrastructure.Repository;

public class VolunteerRequestsRepository : IVolunteerRequestsRepository
{
    private readonly WriteDbContext _context;
    
    public VolunteerRequestsRepository(WriteDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<VolunteerRequestId>> Create(
        VolunteerRequest entity, CancellationToken cancellationToken = default)
    {
        await _context.VolunteerRequests.AddAsync(entity, cancellationToken);

        return entity.Id;
    }

    public async Task<Result<VolunteerRequest>> GetById(VolunteerRequestId id, CancellationToken cancellationToken = default)
    {
        var volunteerRequest = await _context.VolunteerRequests
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        if (volunteerRequest == null)
            return Errors.General.NotFound();

        return volunteerRequest;
    }

    public Result<VolunteerRequestId> Delete(VolunteerRequest entity)
    { 
        _context.VolunteerRequests.Remove(entity);

        return entity.Id;
    }
}