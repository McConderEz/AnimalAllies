namespace NotificationService.Contracts.Requests;

public record SendNotificationTakeRequestForRevisionEvent(Guid UserId, string UserEmail);
