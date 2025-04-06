using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotService.States;

public interface IState
{
    Task<IState> HandleAsync(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken = default);
}