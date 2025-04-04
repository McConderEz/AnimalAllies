namespace NotificationService.Contracts.Requests;

public record SendNotificationRejectVolunteerRequestEvent(Guid UserId, string UserEmail, string RejectMessage);