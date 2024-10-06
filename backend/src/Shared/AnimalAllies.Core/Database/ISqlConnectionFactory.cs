using System.Data;

namespace AnimalAllies.Core.Database;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}