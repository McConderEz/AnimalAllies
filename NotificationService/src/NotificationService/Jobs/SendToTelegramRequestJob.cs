using Hangfire;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Services;

namespace NotificationService.Jobs;

public class SendToTelegramRequestJob(
    TelegramBotHttpClient client,
    ILogger<SendToTelegramRequestJob> logger)
{
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = [5, 10, 15])]
    public async Task Execute(IEnumerable<long> chatIds, string message)
    {
        try
        {
            var telegramBotData = new TelegramBotData(chatIds, message);

            await client.Execute(telegramBotData);
            
            logger.LogInformation("request sent to telegram bot");
        }
        catch (Exception ex)
        {
            logger.LogError("Cannot send request, ex: {ex}", ex.Message);
        }
    }
}