using System.Text.Json;

namespace AnimalAllies.Core.Outbox;

public class OutboxRepository
{
    private readonly OutboxContext _context;

    public OutboxRepository(OutboxContext context)
    {
        _context = context;
    }

    public async Task Add<T>(T message, CancellationToken cancellationToken = default)
    {
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.UtcNow,
            Type = typeof(T).FullName!,
            Payload = JsonSerializer.Serialize(message)
        };
        
        await _context.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}