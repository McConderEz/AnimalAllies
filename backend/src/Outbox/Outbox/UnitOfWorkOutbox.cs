using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Outbox.Abstractions;
using Outbox.Outbox;

namespace Outbox;

public class UnitOfWorkOutbox: IUnitOfWorkOutbox
{
    private readonly OutboxContext _dbContext;

    public UnitOfWorkOutbox(OutboxContext dbContext)
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