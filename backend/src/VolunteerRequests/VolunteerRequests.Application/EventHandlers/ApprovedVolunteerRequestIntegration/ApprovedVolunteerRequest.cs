using AnimalAllies.Core.Database;
using AnimalAllies.SharedKernel.Constraints;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Outbox.Abstractions;
using VolunteerRequests.Contracts.Messaging;
using VolunteerRequests.Domain.Events;

namespace VolunteerRequests.Application.EventHandlers.ApprovedVolunteerRequestIntegration;

public class ApprovedVolunteerRequest: INotificationHandler<ApprovedVolunteerRequestDomainEvent>
{
    private readonly ILogger<ApprovedVolunteerRequest> _logger;
    private readonly IOutboxRepository _outboxRepository;
    private readonly IUnitOfWorkOutbox _unitOfWork;

    public ApprovedVolunteerRequest(
        ILogger<ApprovedVolunteerRequest> logger, 
        IOutboxRepository outboxRepository,
        IUnitOfWorkOutbox unitOfWork)
    {
        _logger = logger;
        _outboxRepository = outboxRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task Handle(ApprovedVolunteerRequestDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new ApprovedVolunteerRequestEvent(
            notification.UserId,
            notification.FirstName,
            notification.SecondName,
            notification.Patronymic,
            notification.WorkExperience);
        
        await _outboxRepository.AddAsync(integrationEvent, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Sent integration event for creation volunteer account for user with id {userId}",
            notification.UserId);
    }
}