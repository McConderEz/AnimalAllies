using MassTransit;
using NotificationService.Contracts.Requests;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Services;
using NotificationService.Validators;

namespace NotificationService.Features.Consumers;

public class RejectVolunteerRequestEventConsumer: IConsumer<SendNotificationRejectVolunteerRequestEvent>
{
    private readonly MailSenderService _mailService;
    private readonly ILogger<RejectVolunteerRequestEventConsumer> _logger;
    private readonly EmailValidator _emailValidator;

    public RejectVolunteerRequestEventConsumer(
        MailSenderService mailService,
        ILogger<RejectVolunteerRequestEventConsumer> logger,
        EmailValidator emailValidator)
    {
        _mailService = mailService;
        _logger = logger;
        _emailValidator = emailValidator;
    }

    public async Task Consume(ConsumeContext<SendNotificationRejectVolunteerRequestEvent> context)
    {
        var message = context.Message;

        var validationResult = _emailValidator.Execute([message.UserEmail]);
        if (validationResult.IsFailure)
            throw new Exception("Invalid email format");

        var mailData = new MailData([message.UserEmail], "Отказ на заявку волонтёра",
            "К сожалению, мы вынуждены вам отказать в волонтёрстве, по причине:" + message.RejectMessage);

        await _mailService.Send(mailData);
        
        _logger.LogInformation("Sent mail with reject volunteer request notification to user {email}",
            message.UserEmail);
    }
}