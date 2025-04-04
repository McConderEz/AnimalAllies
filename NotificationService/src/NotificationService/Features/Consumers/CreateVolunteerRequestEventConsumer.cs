using MassTransit;
using NotificationService.Contracts.Requests;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Services;
using NotificationService.Validators;

namespace NotificationService.Features.Consumers;

public class CreateVolunteerRequestEventConsumer: IConsumer<SendNotificationCreateVolunteerRequestEvent>
{
    private readonly MailSenderService _mailService;
    private readonly ILogger<CreateVolunteerRequestEventConsumer> _logger;
    private readonly EmailValidator _emailValidator;

    public CreateVolunteerRequestEventConsumer(
        MailSenderService mailService,
        ILogger<CreateVolunteerRequestEventConsumer> logger,
        EmailValidator emailValidator)
    {
        _mailService = mailService;
        _logger = logger;
        _emailValidator = emailValidator;
    }

    public async Task Consume(ConsumeContext<SendNotificationCreateVolunteerRequestEvent> context)
    {
        var message = context.Message;

        var validationResult = _emailValidator.Execute([message.UserEmail]);
        if (validationResult.IsFailure)
            throw new Exception("Invalid email format");

        var mailData = new MailData([message.UserEmail], "Создание заявки на волонтёра",
            "Вы создали заявку на волонтёра, мы сообщим, когда она попадёт на рассмотрение");

        await _mailService.Send(mailData);
        
        _logger.LogInformation("Sent mail with create volunteer request notification to user {email}",
            message.UserEmail);
    }
}