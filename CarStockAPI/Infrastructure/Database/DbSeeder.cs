using Dapper;

namespace CarStockAPI.Infrastructure.Database;

/// <summary>
/// Class responsible for seeding the database
/// </summary>
/// <param name="factory">The factory for creating database connections, used to execute the SQL commands for seeding data.</param>
public class DbSeeder(DbConnectionFactory factory)
{
    /// <summary>
    /// Seeds the database with initial data if the Users table is empty. It inserts sample users and cars into the database to provide initial data for testing and development purposes.
    /// </summary>
    public async Task SeedAsync()
    {
        using var connection = factory.CreateConnection();

        var count = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM Users");

        if (count > 0)
            return;

        await connection.ExecuteAsync("""
            INSERT INTO Users (Username, PasswordHash)
            VALUES 
            ('user1', @Password),
            ('user2', @Password),
            ('user3', @Password)
        """, new
        {
            Password = BCrypt.Net.BCrypt.HashPassword("password")
        });

        await connection.ExecuteAsync("""
            INSERT INTO Cars (UserId, Make, Model, Year, Stock)
            VALUES
            (1, 'Toyota', 'Camry', 2020, 5),
            (1, 'Honda', 'Civic', 2019, 3),
            (2, 'BMW', 'X5', 2022, 2),
            (3, 'Ford', 'Mustang', 2021, 4)
        """);
    }
}
