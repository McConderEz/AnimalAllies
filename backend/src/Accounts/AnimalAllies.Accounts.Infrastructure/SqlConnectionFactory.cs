using System.Data;
using AnimalAllies.Core.Database;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace AnimalAllies.Accounts.Infrastructure;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;
    
    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection Create()
        => new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
}