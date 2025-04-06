using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotService.Infrastructure.Repository;

namespace TelegramBotService.States.Authorize;

public class WaitingForPasswordState(RedisUserStateRepository repository): IState
{
    public async Task<IState> HandleAsync(
        Message message,
        ITelegramBotClient botClient,
        CancellationToken cancellationToken = default)
    {
        await repository.SetDataAsync(message.Chat.Id, message.Text, "password", cancellationToken);
        
        return StateFactory.GetState(typeof(WaitingCommandState).FullName!);
    }
}