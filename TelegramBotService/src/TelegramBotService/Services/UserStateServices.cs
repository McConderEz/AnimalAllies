using TelegramBotService.States;

namespace TelegramBotService.Services;

public class UserStateServices
{
    private readonly Dictionary<long, IState> _userStates = new();

    public IState GetState(long chatId)
    {
        return _userStates.GetValueOrDefault(chatId, new StartState());
    }

    public void SetState(long chatId, IState state)
    {
        _userStates[chatId] = state;
    }
}