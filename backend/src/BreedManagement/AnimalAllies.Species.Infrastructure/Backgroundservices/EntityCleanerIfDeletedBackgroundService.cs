using AnimalAllies.Species.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Species.Infrastructure.Backgroundservices;

public class EntityCleanerIfDeletedBackgroundService : BackgroundService
{
    private const int FREQUENCY_OF_DELETION = 24;
    private readonly ILogger<EntityCleanerIfDeletedBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public EntityCleanerIfDeletedBackgroundService(
        ILogger<EntityCleanerIfDeletedBackgroundService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("EntityCleanerIfDeletedBackgroundService is starting");
        
        await using var scope = _scopeFactory.CreateAsyncScope();
        
        var deleteExpiredBreedsService = scope.ServiceProvider.GetRequiredService<DeleteExpiredBreedsService>();
        var deleteExpiredSpeciesService = scope.ServiceProvider.GetRequiredService<DeleteExpiredSpeciesService>();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("EntityCleanerIfDeletedBackgroundService is working");
            await deleteExpiredBreedsService.Process(stoppingToken);
            await deleteExpiredSpeciesService.Process(stoppingToken);
            
            await Task.Delay(TimeSpan.FromDays(FREQUENCY_OF_DELETION), stoppingToken);
        }

        await Task.CompletedTask;
    }
}