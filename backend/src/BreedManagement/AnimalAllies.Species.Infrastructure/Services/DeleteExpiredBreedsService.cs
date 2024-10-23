using AnimalAllies.Core.Options;
using AnimalAllies.Species.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AnimalAllies.Species.Infrastructure.Services;

public class DeleteExpiredBreedsService
{
    private readonly WriteDbContext _dbContext;
    private readonly EntityDeletion _entityDeletion;

    public DeleteExpiredBreedsService(
        WriteDbContext dbContext,
        IOptions<EntityDeletion> entityDeletion)
    {
        _dbContext = dbContext;
        _entityDeletion = entityDeletion.Value;
    }

    public async Task Process(CancellationToken cancellationToken = default)
    {
        var speciesList = await GetSpeciesWithBreeds(cancellationToken);
        foreach (var species in speciesList)
        {
            species.DeleteExpiredBreeds(_entityDeletion.ExpiredTime);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<IEnumerable<Domain.Species>> GetSpeciesWithBreeds(CancellationToken cancellationToken)
    {
        return await _dbContext.Species
            .Include(v => v.Breeds).ToListAsync(cancellationToken);
    }
}