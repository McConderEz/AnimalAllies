namespace AnimalAllies.Domain.Models.Common;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}