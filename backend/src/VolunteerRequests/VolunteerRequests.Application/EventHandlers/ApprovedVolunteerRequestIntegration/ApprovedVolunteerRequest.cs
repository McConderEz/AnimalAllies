using AnimalAllies.Core.Outbox;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using VolunteerRequests.Contracts.Messaging;
using VolunteerRequests.Domain.Events;

namespace VolunteerRequests.Application.EventHandlers.ApprovedVolunteerRequestIntegration;

public class ApprovedVolunteerRequest: INotificationHandler<ApprovedVolunteerRequestDomainEvent>
{
    private readonly ILogger<ApprovedVolunteerRequest> _logger;
    private readonly OutboxRepository _outboxRepository;

    public ApprovedVolunteerRequest(
        ILogger<ApprovedVolunteerRequest> logger, 
        OutboxRepository outboxRepository)
    {
        _logger = logger;
        _outboxRepository = outboxRepository;
    }


    public async Task Handle(ApprovedVolunteerRequestDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new ApprovedVolunteerRequestEvent(
            notification.UserId,
            notification.FirstName,
            notification.SecondName,
            notification.Patronymic,
            notification.WorkExperience);
        
        await _outboxRepository.Add(integrationEvent, cancellationToken);
        
        _logger.LogInformation("Sent integration event for creation volunteer account for user with id {userId}",
            notification.UserId);
    }
}