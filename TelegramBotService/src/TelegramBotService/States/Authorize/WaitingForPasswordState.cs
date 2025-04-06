using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotService.States.Authorize;

public class WaitingForPasswordState: IState
{
    public async Task<IState> HandleAsync(
        Message message,
        ITelegramBotClient botClient,
        CancellationToken cancellationToken = default)
    {
        return new WaitingCommandState();
    }
}