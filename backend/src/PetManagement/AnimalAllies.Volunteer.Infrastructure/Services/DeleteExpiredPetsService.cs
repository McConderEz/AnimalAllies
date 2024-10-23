using AnimalAllies.Core.Options;
using AnimalAllies.Volunteer.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AnimalAllies.Volunteer.Infrastructure.Services;

public class DeleteExpiredPetsService
{
    private readonly WriteDbContext _dbContext;
    private readonly EntityDeletion _entityDeletion;

    public DeleteExpiredPetsService(
        WriteDbContext dbContext,
        IOptions<EntityDeletion> entityDeletion)
    {
        _dbContext = dbContext;
        _entityDeletion = entityDeletion.Value;
    }

    public async Task Process(CancellationToken cancellationToken = default)
    {
        var volunteers = await GetVolunteersWithPets(cancellationToken);
        foreach (var volunteer in volunteers)
        {
            volunteer.DeleteExpiredPets(_entityDeletion.ExpiredTime);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<IEnumerable<Domain.VolunteerManagement.Aggregate.Volunteer>> GetVolunteersWithPets(CancellationToken cancellationToken)
    {
        return await _dbContext.Volunteers
            .Include(v => v.Pets).ToListAsync(cancellationToken);
    }
}