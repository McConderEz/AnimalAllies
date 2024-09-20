using System.Data;

namespace AnimalAllies.Application.Database;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}