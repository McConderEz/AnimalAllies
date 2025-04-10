using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Outbox.Abstractions;

namespace Outbox.Outbox;

public class OutboxRepository<TContext> : IOutboxRepository where TContext : DbContext
{
    private readonly TContext _context;

    public OutboxRepository(TContext context)
    {
        _context = context;
    }

    public async Task AddAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.UtcNow,
            Type = typeof(T).FullName!,
            Payload = JsonSerializer.Serialize(message)
        };
        
        await _context.Set<OutboxMessage>().AddAsync(outboxMessage, cancellationToken);
    }
}