namespace NotificationService.Contracts.Requests;

public record SendNotificationCreateVolunteerRequestEvent(Guid UserId, string UserEmail);
