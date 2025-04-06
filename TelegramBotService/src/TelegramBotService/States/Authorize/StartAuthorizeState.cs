using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotService.States.Authorize;

public class StartAuthorizeState: IState
{
    public async Task<IState> HandleAsync(
        Message message,
        ITelegramBotClient botClient,
        CancellationToken cancellationToken = default)
    {
        await botClient.SendMessage(
            message.Chat.Id, 
            "Введите ваш email:", 
            cancellationToken: cancellationToken);

        return new WaitingForEmailState();
    }
}