using AnimalAllies.SharedKernel.Shared.ValueObjects;

namespace AnimalAllies.Core.Options;

public class RefreshSessionOptions
{
    public static string REFRESH_SESSION = nameof(REFRESH_SESSION);
    
    public int ExpiredDaysTime { get; init; }
}