using NotificationService.Api.Enpoints;
using NotificationService.Infrastructure.Services;
using NotificationService.Validators;

namespace NotificationService.Features.Sending;

public class SendEmailConfirmTokenByEmail
{
    private record SubscribeOnEmailNotificationsRequest(
        string Reciever, 
        string Subject,
        string Body);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("sending-confirmation-by-email", Handler);
        }
    }
    
    /// <summary>
    /// Обработчик отправления уведомлений с фильтрацией
    /// </summary>
    /// <param name="request">Принимаемый запрос</param>
    /// <param name="service">Сервис отправки почтовых сообщений</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    private static async Task<IResult> Handler( 
        SubscribeOnEmailNotificationsRequest request,
        MailSenderService service,
        EmailValidator validator,
        CancellationToken cancellationToken = default)
    {
        List<string> recievers = [request.Reciever];

        var validationResult = validator.Execute(recievers);
        if (validationResult.IsFailure)
            return Results.BadRequest(validationResult.Error);
        
        
        
        return Results.Ok();
    }
}