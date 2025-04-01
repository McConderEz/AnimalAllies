namespace NotificationService.Contracts.Requests;

public record SendConfirmTokenByEmailRequest(Guid UserId, string Email, string Code);
