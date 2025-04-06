using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotService.States.Authorize;

public class WaitingForEmailState: IState
{
    public async Task<IState> HandleAsync(
        Message message,
        ITelegramBotClient botClient, 
        CancellationToken cancellationToken = default)
    {
        await botClient.SendMessage(
            message.Chat.Id, 
            "Введите ваш пароль:", 
            cancellationToken: cancellationToken);

        return new WaitingForPasswordState();
    }
}