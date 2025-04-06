using Microsoft.Extensions.Caching.Hybrid;
using TelegramBotService.States;

namespace TelegramBotService.Infrastructure.Repository;

public class RedisUserStateRepository
{
    private readonly HybridCache _hybridCache;

    public RedisUserStateRepository(HybridCache hybridCache)
    {
        _hybridCache = hybridCache;
    }

    public async Task<IState> GetOrCreateStateAsync(
        long chatId,
        CancellationToken cancellationToken = default)
    {
        var stateName = await _hybridCache.GetOrCreateAsync(
            $"user:{chatId}:state",
            factory: async _ => nameof(StartState),
            cancellationToken: cancellationToken);

        return StateFactory.GetState(stateName);
    }

    public async Task SetStateAsync(long chatId, IState state, CancellationToken cancellationToken = default)
    {
        var stateName = state.GetType().Name;
        
        await _hybridCache.SetAsync(
            key: $"user:{chatId}:state",
            value: stateName,
            cancellationToken: cancellationToken);
    }
}