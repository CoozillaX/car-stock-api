using Dapper;
using car_stock_api.Domain.Entities;
using car_stock_api.Infrastructure.Database;

namespace car_stock_api.Infrastructure.Repositories;

/// <summary>
/// Repository class for managing User entities in the database.
/// </summary>
/// <param name="dbConnectionFactory">
/// The factory for creating database connections.
/// </param>
public class UserRepository(DbConnectionFactory dbConnectionFactory)
{
    private readonly DbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    /// <summary>
    /// Creates a new user in the database and returns the created user with its assigned Id.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <returns>The created user with Id</returns>
    public async Task<User> CreateAsync(User user)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var id = await connection.ExecuteScalarAsync<int>(
            @"
            INSERT INTO Users (Username, PasswordHash) 
            VALUES (@Username, @PasswordHash);
            SELECT last_insert_rowid();
            ",
            user
        );
        user.Id = id;
        return user;
    }

    /// <summary>
    /// Removes a user from the database by its Id.
    /// Returns true if the user was successfully removed, false otherwise.
    /// </summary>
    /// <param name="id">The Id of the user to remove.</param>
    /// <returns>If the user was successfully removed.</returns>
    public async Task<bool> RemoveAsync(int id)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(
            @"DELETE FROM Users WHERE Id = @Id;",
            new { Id = id }
        );
        return rows > 0;
    }

    /// <summary>
    /// Retrieves a user from the database that matches the given username.
    /// Since usernames are unique, this will return either a single user or null.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <returns>A user that matches the given username, or null if no such user exists.</returns>
    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(
            @"SELECT * FROM Users WHERE Username = @Username;",
            new { Username = username }
        );
    }
}
