using System.Text;
using Newtonsoft.Json;
using NotificationService.Domain.Models;

namespace NotificationService.Infrastructure.Services;

/// <summary>
/// Класс для отправки http запросов на телеграм-бота с веб-хуками
/// </summary>
/// <param name="logger"></param>
public class TelegramBotHttpClient(
    IConfiguration configuration,
    IHttpClientFactory _httpClientFactory,
    ILogger<TelegramBotHttpClient> logger)
{
    /// <summary>
    /// Отправка http запроса телеграмм-боту с информацией о событии
    /// </summary>
    /// <param name="telegramBotData">данные для отправки телеграмм-боту</param>
    /// <param name="cancellationToken">токен отмены</param>
    public async Task Execute(
        TelegramBotData telegramBotData ,CancellationToken cancellationToken = default)
    {
        using var client = _httpClientFactory.CreateClient();

        var token = configuration.GetConnectionString("TelegramBotToken");
            
        var jsonData = JsonConvert.SerializeObject(telegramBotData);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var route = $"https://api.telegram.org/bot{token}/sendMessage";
            
        using var request = new HttpRequestMessage(HttpMethod.Post, route);
        request.Content = content;
            
        var response = await client.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogInformation($"Response: {responseData}");
        }
        else
        {
            logger.LogError($"Error: {response.StatusCode}");
        }
    }
}