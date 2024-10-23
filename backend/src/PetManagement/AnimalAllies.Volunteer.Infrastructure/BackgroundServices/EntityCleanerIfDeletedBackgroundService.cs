using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.Volunteer.Infrastructure.DbContexts;
using AnimalAllies.Volunteer.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Volunteer.Infrastructure.BackgroundServices;

public class EntityCleanerIfDeletedBackgroundService : BackgroundService
{
    private const int FREQUENCY_OF_DELETION = 24;
    private readonly ILogger<FilesCleanerBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public EntityCleanerIfDeletedBackgroundService(
        ILogger<FilesCleanerBackgroundService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("EntityCleanerIfDeletedBackgroundService is starting");
        
        await using var scope = _scopeFactory.CreateAsyncScope();
        
        var deleteExpiredPetsService = scope.ServiceProvider.GetRequiredService<DeleteExpiredPetsService>();
        var deleteExpiredVolunteerService = scope.ServiceProvider.GetRequiredService<DeleteExpiredVolunteerService>();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("EntityCleanerIfDeletedBackgroundService is working");
            await deleteExpiredPetsService.Process(stoppingToken);
            await deleteExpiredVolunteerService.Process(stoppingToken);
            
            await Task.Delay(TimeSpan.FromHours(FREQUENCY_OF_DELETION), stoppingToken);
        }

        await Task.CompletedTask;
    }
}