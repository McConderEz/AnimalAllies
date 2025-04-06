namespace TelegramBotService.States;

using TelegramBotService.States.Authorize;

public static class StateFactory
{
    private static readonly Dictionary<string, Func<IState>> _stateDictionary = new()
    {
        { nameof(StartState), () => new StartState() },
        { nameof(WaitingCommandState), () => new WaitingCommandState() },
        { nameof(StartAuthorizeState), () => new StartAuthorizeState() },
        { nameof(WaitingForEmailState), () => new WaitingForEmailState() },
        { nameof(WaitingForPasswordState), () => new WaitingForPasswordState() }
    };

    public static IState GetState(string stateName)
    {
        if (_stateDictionary.TryGetValue(stateName, out var stateFactory))
        {
            return stateFactory();
        }

        throw new InvalidOperationException($"State '{stateName}' is not registered.");
    }
}