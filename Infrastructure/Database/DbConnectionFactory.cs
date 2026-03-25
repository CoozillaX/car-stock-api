using System.Data;
using Microsoft.Data.Sqlite;

namespace car_stock_api.Infrastructure.Database;

/// <summary>
/// Factory class for creating SQLite database connections.
/// </summary>
/// <param name="configuration">The application configuration, used to retrieve the connection string.</param>
public class DbConnectionFactory(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public IDbConnection CreateConnection()
    {
        var connectionString = _configuration.GetConnectionString("Default");

        return new SqliteConnection(connectionString);
    }
}
