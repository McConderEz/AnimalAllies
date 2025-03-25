using NotificationService.Api.Enpoints;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Services;

namespace NotificationService.Features;

public class SendEmailNotificationByFilter
{
    private record SendEmailNotificationByFilterRequest(IEnumerable<string> Recievers, string Subject, string Body);
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("sending", Handler);
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
        SendEmailNotificationByFilterRequest request,
        MailSenderService service,
        CancellationToken cancellationToken = default)
    {
        var mailData = new MailData(request.Recievers, request.Subject, request.Body);
        
        var result = await service.Send(mailData);
        
        return Results.Ok();
    }
}