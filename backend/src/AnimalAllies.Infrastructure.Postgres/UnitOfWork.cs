using System.Data;
using AnimalAllies.Application.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace AnimalAllies.Infrastructure;

public class UnitOfWork: IUnitOfWork
{
    private readonly AnimalAlliesDbContext _dbContext;

    public UnitOfWork(AnimalAlliesDbContext dbContext)
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