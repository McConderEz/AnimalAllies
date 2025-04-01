using MassTransit;
using Microsoft.EntityFrameworkCore;
using NotificationService.Contracts.Requests;
using NotificationService.Infrastructure.DbContext;
using NotificationService.Options;

namespace NotificationService.Features.Consumers;

public class SetStartUserNotificationSettingsEvent: IConsumer<SetStartUserNotificationSettingsRequest>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<SetStartUserNotificationSettingsEvent> _logger;

    public SetStartUserNotificationSettingsEvent(
        ApplicationDbContext dbContext,
        ILogger<SetStartUserNotificationSettingsEvent> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SetStartUserNotificationSettingsRequest> context)
    {
        var message = context.Message;

        var userNotificationsSettingsEvent =
            await _dbContext.UserNotificationSettings
                .FirstOrDefaultAsync(s => s.UserId == message.UserId, context.CancellationToken);

        if (userNotificationsSettingsEvent is not null)
            return;

        userNotificationsSettingsEvent = new UserNotificationSettings
        {
            Id = Guid.NewGuid(),
            UserId = message.UserId,
            EmailNotifications = true,
            TelegramNotifications = false,
            WebNotifications = false
        };

        await _dbContext.UserNotificationSettings.AddAsync(userNotificationsSettingsEvent, context.CancellationToken);
        await _dbContext.SaveChangesAsync(context.CancellationToken);
        
        _logger.LogInformation("Created notification settings for user {userid}", message.UserId);
    }
}