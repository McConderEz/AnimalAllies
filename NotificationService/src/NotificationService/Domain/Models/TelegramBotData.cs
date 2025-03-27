namespace NotificationService.Domain.Models;

/// <summary>
/// Данные для отправки в телеграм-бот
/// </summary>
public class TelegramBotData
{
    public TelegramBotData(IEnumerable<long> chatIds, string message)
    {
        ChatIds = chatIds.ToList();
        Message = message;
    }

    public List<long> ChatIds { get; set; } = [];

    public string Message { get; } = string.Empty;
}