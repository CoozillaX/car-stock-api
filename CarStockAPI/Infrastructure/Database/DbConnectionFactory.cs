using System.Data;
using Microsoft.Data.Sqlite;

namespace CarStockAPI.Infrastructure.Database;

/// <summary>
/// Factory class for creating SQLite database connections.
/// </summary>
/// <param name="configuration">The application configuration, used to retrieve the connection string.</param>
public class DbConnectionFactory(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    /// <summary>
    /// Creates and returns a new SQLite database connection using the connection string from the configuration.
    /// </summary>
    public IDbConnection CreateConnection()
    {
        var connectionString = _configuration.GetConnectionString("Default");

        return new SqliteConnection(connectionString);
    }
}
