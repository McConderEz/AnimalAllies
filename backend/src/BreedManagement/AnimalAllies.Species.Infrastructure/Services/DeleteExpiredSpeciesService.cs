using AnimalAllies.Core.Options;
using AnimalAllies.Species.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AnimalAllies.Species.Infrastructure.Services;

public class DeleteExpiredSpeciesService
{
    private readonly WriteDbContext _dbContext;
    private readonly EntityDeletion _entityDeletion;

    public DeleteExpiredSpeciesService(
        WriteDbContext dbContext,
        IOptions<EntityDeletion> entityDeletion)
    {
        _dbContext = dbContext;
        _entityDeletion = entityDeletion.Value;
    }

    public async Task Process(CancellationToken cancellationToken = default)
    {
        var speciesList = await _dbContext.Species
            .Where(v => v.IsDeleted).ToListAsync(cancellationToken);
        
        foreach (var species in speciesList)
        {
            if (species.DeletionDate!.Value.AddDays(_entityDeletion.ExpiredTime) <= DateTime.UtcNow)
                _dbContext.Species.Remove(species);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}