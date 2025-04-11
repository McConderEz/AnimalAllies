using MassTransit;
using Microsoft.EntityFrameworkCore;
using NotificationService.Contracts.Requests;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.DbContext;
using NotificationService.Infrastructure.Services;
using NotificationService.Validators;
using Outbox.Abstractions;

namespace NotificationService.Features.Consumers;

public class ApproveVolunteerRequestEventConsumer: IConsumer<SendNotificationApproveVolunteerRequestEvent>
{
    private readonly MailSenderService _mailService;
    private readonly ILogger<ApproveVolunteerRequestEventConsumer> _logger;
    private readonly EmailValidator _emailValidator;
    private readonly ApplicationDbContext _context;
    private readonly IOutboxRepository _outboxRepository;
    private readonly IUnitOfWorkOutbox _unitOfWorkOutbox;

    public ApproveVolunteerRequestEventConsumer(
        MailSenderService mailService,
        ILogger<ApproveVolunteerRequestEventConsumer> logger,
        EmailValidator emailValidator,
        ApplicationDbContext context,
        IOutboxRepository outboxRepository,
        IUnitOfWorkOutbox unitOfWorkOutbox)
    {
        _mailService = mailService;
        _logger = logger;
        _emailValidator = emailValidator;
        _context = context;
        _outboxRepository = outboxRepository;
        _unitOfWorkOutbox = unitOfWorkOutbox;
    }

    public async Task Consume(ConsumeContext<SendNotificationApproveVolunteerRequestEvent> context)
    {
        var message = context.Message;

        var validationResult = _emailValidator.Execute([message.UserEmail]);
        if (validationResult.IsFailure)
            throw new Exception("Invalid email format");

        var settings = await _context.UserNotificationSettings.FirstOrDefaultAsync(
            u => u.UserId == message.UserId, context.CancellationToken);
        
        if(settings is null)
            return;

        var description = "Мы одобрили вашу заявку на волонтёрство!Примите наши поздравления!";
        
        if (settings.EmailNotifications)
        {
            var mailData = new MailData(
                [message.UserEmail], 
                "Одобрение заявки на волонтёрство",
                description);

            await _mailService.Send(mailData);
        }

        if (settings.TelegramNotifications)
        {
            var messageEvent = new SendTelegramNotificationEvent(settings.UserId, description);
            
            await _outboxRepository.AddAsync(messageEvent, context.CancellationToken);
            await _unitOfWorkOutbox.SaveChanges(context.CancellationToken);
        }

        _logger.LogInformation("Sent notifications with approved volunteer request notification to user {email}",
            message.UserEmail);
    }
}