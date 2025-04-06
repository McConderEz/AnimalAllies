using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotService.States.Authorize;

namespace TelegramBotService.States;

public class WaitingCommandState: IState
{
    public async Task<IState> HandleAsync(
        Message message,
        ITelegramBotClient botClient,
        CancellationToken cancellationToken = default)
    {
        var state = message.Text switch
        {
            "/start" => new StartState() as IState,
            "/authorize" => new StartAuthorizeState() as IState,
            "/info" => throw new NotImplementedException(),
            "/help" => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        var nextState = await state.HandleAsync(message, botClient, cancellationToken);
        
        return nextState;
    }
}