using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using VolunteerRequests.Contracts.Messaging;

namespace VolunteerRequests.Application.EventHandlers.ApprovedVolunteerRequestIntegration;

public class ApprovedVolunteerRequest: INotificationHandler<ApprovedVolunteerRequestEvent>
{
    private readonly ILogger<ApprovedVolunteerRequest> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public ApprovedVolunteerRequest(
        ILogger<ApprovedVolunteerRequest> logger, 
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }


    public async Task Handle(ApprovedVolunteerRequestEvent notification, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(notification, cancellationToken);
        
        _logger.LogInformation("Sent integration event for creation volunteer account for user with id {userId}",
            notification.UserId);
    }
}