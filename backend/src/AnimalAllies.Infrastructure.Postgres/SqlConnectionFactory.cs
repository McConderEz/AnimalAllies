using System.Data;
using AnimalAllies.Application.Database;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace AnimalAllies.Infrastructure;

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