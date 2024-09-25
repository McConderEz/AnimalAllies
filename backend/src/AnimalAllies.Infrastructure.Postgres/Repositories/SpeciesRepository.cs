using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models.Species;
using AnimalAllies.Domain.Shared;
using AnimalAllies.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace AnimalAllies.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly WriteDbContext _context;

    public SpeciesRepository(WriteDbContext context)
    {
        _context = context;
    }

    public async Task<Result<SpeciesId>> Create(Species entity, CancellationToken cancellationToken = default)
    {
        await _context.Species.AddAsync(entity, cancellationToken);

        return entity.Id;
    }

    public Result<SpeciesId> Delete(Species entity, CancellationToken cancellationToken = default)
    {
        _context.Species.Remove(entity);

        return entity.Id;
    }

    public Result<SpeciesId> Save(Species entity, CancellationToken cancellationToken = default)
    {
        _context.Species.Attach(entity);

        return entity.Id;
    }

    public async Task<Result<Species>> GetById(SpeciesId id, CancellationToken cancellationToken = default)
    {
        var species = await _context.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (species == null)
            return Result<Species>.Failure(Errors.General.NotFound());

        return Result<Species>.Success(species);
    }

    public async Task<Result<List<Species>>> Get(CancellationToken cancellationToken = default)
    {
        return await _context.Species
            .Include(s => s.Breeds)
            .ToListAsync(cancellationToken);
    }
}