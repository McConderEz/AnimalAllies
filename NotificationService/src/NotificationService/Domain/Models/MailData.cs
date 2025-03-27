namespace NotificationService.Domain.Models;

/// <summary>
/// Данные для отправки по почте
/// </summary>
public class MailData
{
    public MailData(IEnumerable<string> recievers, string subject, string body)
    {
        To = recievers.ToList();
        Subject = subject;
        Body = body;
    }

    public List<string> To { get; set; } = [];

    public string Subject { get; } = string.Empty;

    public string Body { get; } = string.Empty; 
}