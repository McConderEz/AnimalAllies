namespace NotificationService.Contracts.Requests;

public record SendNotificationApproveVolunteerRequestEvent(Guid UserId, string UserEmail);
