using Dapper;

namespace car_stock_api.Infrastructure.Database;

/// <summary>
/// Class responsible for initializing the database schema by creating necessary tables if they do not exist.
/// </summary>
/// <param name="dbConnectionFactory">
/// The factory for creating database connections, used to execute the SQL commands for table creation.
/// </param>
public class DBInitialiser(DbConnectionFactory dbConnectionFactory)
{
    private readonly DbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    /// <summary>
    /// Initializes the database by creating the Users and Cars tables if they do not already exist.
    /// The Users table is created first since the Cars table has a foreign key reference to it.
    /// </summary>
    public async Task InitialiseAsync()
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        // Create Users table first, since Cars has a foreign key reference to it
        await connection.ExecuteAsync("""
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL UNIQUE,
                PasswordHash TEXT NOT NULL
            );
        """);

        // Create Cars table
        await connection.ExecuteAsync("""
            CREATE TABLE IF NOT EXISTS Cars (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId INTEGER NOT NULL,
                Make TEXT NOT NULL,
                Model TEXT NOT NULL,
                Year INTEGER NOT NULL,
                Stock INTEGER NOT NULL,
                UNIQUE (UserId, Make, Model, Year),
                FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
            );
        """);
    }
}
