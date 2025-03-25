namespace NotificationService.Application.Abstraction;

public interface IMigrator
{
    Task Migrate(CancellationToken cancellationToken = default);
}