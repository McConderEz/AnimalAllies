using MassTransit;
using NotificationService.Contracts.Requests;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Services;
using NotificationService.Validators;

namespace NotificationService.Features.Consumers;

public class ApproveVolunteerRequestEventConsumer: IConsumer<SendNotificationApproveVolunteerRequestEvent>
{
    private readonly MailSenderService _mailService;
    private readonly ILogger<ApproveVolunteerRequestEventConsumer> _logger;
    private readonly EmailValidator _emailValidator;

    public ApproveVolunteerRequestEventConsumer(
        MailSenderService mailService,
        ILogger<ApproveVolunteerRequestEventConsumer> logger,
        EmailValidator emailValidator)
    {
        _mailService = mailService;
        _logger = logger;
        _emailValidator = emailValidator;
    }

    public async Task Consume(ConsumeContext<SendNotificationApproveVolunteerRequestEvent> context)
    {
        var message = context.Message;

        var validationResult = _emailValidator.Execute([message.UserEmail]);
        if (validationResult.IsFailure)
            throw new Exception("Invalid email format");

        var mailData = new MailData([message.UserEmail], "Одобрение заявки на волонтёрство",
            "Мы одобрили вашу заявку на волонтёрство!Примите наши поздравления!");

        await _mailService.Send(mailData);
        
        _logger.LogInformation("Sent mail with approved volunteer request notification to user {email}",
            message.UserEmail);
    }
}