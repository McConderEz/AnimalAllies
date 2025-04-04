namespace NotificationService.Contracts.Requests;

public record SendConfirmTokenByEmailEvent(Guid UserId, string Email, string Code);
