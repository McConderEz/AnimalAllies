using AnimalAllies.Core.Options;
using AnimalAllies.Volunteer.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AnimalAllies.Volunteer.Infrastructure.Services;

public class DeleteExpiredVolunteerService
{
    private readonly WriteDbContext _dbContext;
    private readonly EntityDeletion _entityDeletion;

    public DeleteExpiredVolunteerService(
        WriteDbContext dbContext,
        IOptions<EntityDeletion> entityDeletion)
    {
        _dbContext = dbContext;
        _entityDeletion = entityDeletion.Value;
    }

    public async Task Process(CancellationToken cancellationToken = default)
    {
        var volunteers = await _dbContext.Volunteers
            .Where(v => v.IsDeleted).ToListAsync(cancellationToken);
        
        foreach (var volunteer in volunteers)
        {
            if (volunteer.DeletionDate!.Value.AddDays(_entityDeletion.ExpiredTime) <= DateTime.UtcNow)
                _dbContext.Volunteers.Remove(volunteer);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}