namespace AnimalAllies.Infrastructure.Common;

public class BaseRepository
{
    private protected readonly AnimalAlliesDbContext _context;
    private protected readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public BaseRepository(AnimalAlliesDbContext context)
    {
        _context = context;
    }
}