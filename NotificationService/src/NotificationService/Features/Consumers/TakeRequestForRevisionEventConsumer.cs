using MassTransit;
using NotificationService.Contracts.Requests;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Services;
using NotificationService.Validators;

namespace NotificationService.Features.Consumers;

public class TakeRequestForRevisionEventConsumer: IConsumer<SendNotificationTakeRequestForRevisionEvent>
{
    private readonly MailSenderService _mailService;
    private readonly ILogger<TakeRequestForRevisionEventConsumer> _logger;
    private readonly EmailValidator _emailValidator;

    public TakeRequestForRevisionEventConsumer(
        MailSenderService mailService,
        ILogger<TakeRequestForRevisionEventConsumer> logger,
        EmailValidator emailValidator)
    {
        _mailService = mailService;
        _logger = logger;
        _emailValidator = emailValidator;
    }

    public async Task Consume(ConsumeContext<SendNotificationTakeRequestForRevisionEvent> context)
    {
        var message = context.Message;

        var validationResult = _emailValidator.Execute([message.UserEmail]);
        if (validationResult.IsFailure)
            throw new Exception("Invalid email format");

        var mailData = new MailData([message.UserEmail], "Рассмотрение заявки",
            "Ваша заявка принята на рассмотрение нашим сотрудником, мы сообщим вам о результатах");

        await _mailService.Send(mailData);
        
        _logger.LogInformation("Sent mail with take for revision volunteer request notification to user {email}",
            message.UserEmail);
    }
}