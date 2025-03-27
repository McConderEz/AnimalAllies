using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Abstraction;
using NotificationService.Infrastructure.DbContext;

namespace NotificationService.Infrastructure;

/// <summary>
/// Класс для автоматических миграций базы данных
/// </summary>
/// <param name="context">контекст базы данных</param>
/// <param name="logger">класс для логгирования</param>
public class Migrator(ApplicationDbContext context, ILogger<ApplicationDbContext> logger): IMigrator
{
    /// <summary>
    /// Запуск автоматических миграций:
    /// если подключение не удалось - создаётся база данных, после применяются миграции
    /// </summary>
    /// <param name="cancellationToken">токен отмены</param>
    public async Task Migrate(CancellationToken cancellationToken = default)
    {
        if (await context.Database.CanConnectAsync(cancellationToken) == false)
        {
            await context.Database.EnsureCreatedAsync(cancellationToken);
        }
        
        logger.LogInformation("Applying migrations...");
        await context.Database.MigrateAsync(cancellationToken);
        logger.LogInformation("Migrations applied successfully");
    }
}