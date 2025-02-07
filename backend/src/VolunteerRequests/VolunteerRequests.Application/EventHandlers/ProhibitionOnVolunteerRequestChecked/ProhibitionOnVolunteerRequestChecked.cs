using AnimalAllies.SharedKernel.Exceptions;
using MediatR;
using VolunteerRequests.Application.Repository;
using VolunteerRequests.Domain.Events;

namespace VolunteerRequests.Application.EventHandlers.ProhibitionOnVolunteerRequestChecked;

public class ProhibitionOnVolunteerRequestChecked : INotificationHandler<ProhibitionOnVolunteerRequestCheckedEvent>
{
    private static readonly int REQUEST_BLOCKING_PERIOD = 7;
    
    private readonly IProhibitionSendingRepository _prohibitionSendingRepository;
    
    public ProhibitionOnVolunteerRequestChecked(
        IProhibitionSendingRepository prohibitionSendingRepository)
    {
        _prohibitionSendingRepository = prohibitionSendingRepository;
    }

    public async Task Handle(ProhibitionOnVolunteerRequestCheckedEvent notification, CancellationToken cancellationToken)
    {
        var prohibitionSending = await _prohibitionSendingRepository
            .GetByUserId(notification.UserId, cancellationToken);
        
        if (prohibitionSending.IsSuccess)
        {
            var checkResult = prohibitionSending.Value.CheckExpirationOfProhibtion(REQUEST_BLOCKING_PERIOD);

            if (checkResult.IsFailure)
                throw new AccountBannedException(checkResult.Errors.ToString());
                
            var result = _prohibitionSendingRepository.Delete(prohibitionSending.Value);
            if (result.IsFailure)
                throw new AccountBannedException(checkResult.Errors.ToString());
        }
        
    }
}