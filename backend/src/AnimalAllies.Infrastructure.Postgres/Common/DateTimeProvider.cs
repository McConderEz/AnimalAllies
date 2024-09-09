using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Shared;

namespace AnimalAllies.Infrastructure.Common;

public class DateTimeProvider: IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}