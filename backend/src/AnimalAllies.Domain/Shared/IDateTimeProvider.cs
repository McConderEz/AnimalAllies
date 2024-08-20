namespace AnimalAllies.Domain.Shared;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}