using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotService.Infrastructure.Repository;
using TelegramBotService.States.Authorize;

namespace TelegramBotService.States;

public class WaitingCommandState: IState
{
    public async Task<IState> HandleAsync(
        Message message,
        ITelegramBotClient botClient,
        CancellationToken cancellationToken = default)
    {
        var stateName = message.Text switch
        {
            "/start" => typeof(StartState).FullName,
            "/authorize" => typeof(StartAuthorizeState).FullName,
            "/info" => throw new NotImplementedException(),
            "/help" => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        var state = StateFactory.GetState(stateName!);
        var nextState = await state.HandleAsync(message, botClient, cancellationToken);
        
        return nextState;
    }
}