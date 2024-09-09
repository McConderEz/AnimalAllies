namespace AnimalAllies.Domain.Common;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}