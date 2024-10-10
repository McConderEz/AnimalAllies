using System.Data;
using AnimalAllies.Core.Database;
using AnimalAllies.Species.Application.Database;
using AnimalAllies.Species.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace AnimalAllies.Species.Infrastructure;

public class UnitOfWork: IUnitOfWork
{
    private readonly WriteDbContext _dbContext;

    public UnitOfWork(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}