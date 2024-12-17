using AnimalAllies.SharedKernel.Exceptions;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using MediatR;
using Microsoft.Extensions.Logging;
using VolunteerRequests.Application.Repository;
using VolunteerRequests.Domain.Aggregates;
using VolunteerRequests.Domain.Events;

namespace VolunteerRequests.Application.EventHandlers.VolunteerRequestRejectedEventHandlers;

public class VolunteerRequestRejected: INotificationHandler<VolunteerRequestRejectedEvent>
{
    private readonly ILogger<VolunteerRequestRejected> _logger;
    private readonly IProhibitionSendingRepository _repository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public VolunteerRequestRejected(
        ILogger<VolunteerRequestRejected> logger,
        IProhibitionSendingRepository repository, 
        IDateTimeProvider dateTimeProvider)
    {
        _logger = logger;
        _repository = repository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(VolunteerRequestRejectedEvent notification, CancellationToken cancellationToken)
    {
        
        var prohibitionSendingId = ProhibitionSendingId.NewGuid();
        var prohibitionSending = ProhibitionSending.Create(
            prohibitionSendingId,
            notification.UserId,
            _dateTimeProvider.UtcNow);

        if (prohibitionSending.IsFailure)
            throw new Exception(prohibitionSending.Errors.ToString());
            
        var prohibitionSendingResult = await _repository.Create(prohibitionSending.Value, cancellationToken);

        if (prohibitionSendingResult.IsFailure)
            throw new Exception(prohibitionSendingResult.Errors.ToString());
        
        _logger.LogInformation("User was prohibited for creating request with id {UserId}",
            notification.UserId);
    }
}