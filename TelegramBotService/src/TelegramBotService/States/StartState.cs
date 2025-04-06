using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotService.States;

public class StartState: IState
{
    public async Task<IState> HandleAsync(
        Message message,
        ITelegramBotClient botClient,
        CancellationToken cancellationToken = default)
    {
        await botClient.SendMessage(
            message.Chat.Id, 
            "Добро пожаловать! Введите ваш email:", 
            cancellationToken: cancellationToken);

        return new WaitingForEmailState();
    }
}