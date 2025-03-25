using Hangfire;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Services;

namespace NotificationService.Jobs;

public class SendEmailJob(
    MailSenderService service,
    ILogger<SendEmailJob> logger)
{
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = [5, 10, 15])]
    public async Task Execute(IEnumerable<string> recievers, string subject, string body)
    {
        try
        {
            var mailData = new MailData(recievers, subject, body);

            var result = await service.Send(mailData);
            if (result.IsFailure)
            {
                logger.LogError(result.Error);
                return;
            }

            logger.LogInformation("Mail sent to reciever");
        }
        catch (Exception ex)
        {
            logger.LogError("Cannot send email, ex: {ex}", ex.Message);
        }
    }
}