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
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The created user with Id</returns>
    public async Task<User> CreateAsync(User user, CancellationToken ct = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var command = new CommandDefinition(
            """
            INSERT INTO Users (Username, PasswordHash) 
            VALUES (@Username, @PasswordHash);
            SELECT last_insert_rowid();
            """,
            user,
            cancellationToken: ct
        );
        
        var id = await connection.ExecuteScalarAsync<int>(
            command
        );
        user.Id = id;
        return user;
    }

    /// <summary>
    /// Removes a user from the database by its Id.
    /// Returns true if the user was successfully removed, false otherwise.
    /// </summary>
    /// <param name="id">The Id of the user to remove.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>If the user was successfully removed.</returns>
    public async Task<bool> RemoveAsync(int id, CancellationToken ct = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var command = new CommandDefinition(
            "DELETE FROM Users WHERE Id = @Id;",
            new { Id = id },
            cancellationToken: ct
        );

        var rows = await connection.ExecuteAsync(command);
        return rows > 0;
    }

    /// <summary>
    /// Retrieves a user from the database by its Id. Returns null if no user with the given Id exists.
    /// </summary>
    /// <param name="id">The Id of the user to retrieve.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The user with the specified Id, or null if not found.</returns>
    public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var command = new CommandDefinition(
            "SELECT * FROM Users WHERE Id = @Id;",
            new { Id = id },
            cancellationToken: ct
        );

        return await connection.QueryFirstOrDefaultAsync<User>(command);
    }

    /// <summary>
    /// Retrieves a user from the database that matches the given username.
    /// Since usernames are unique, this will return either a single user or null.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A user that matches the given username, or null if no such user exists.</returns>
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var command = new CommandDefinition(
            "SELECT * FROM Users WHERE Username = @Username;",
            new { Username = username },
            cancellationToken: ct
        );

        return await connection.QueryFirstOrDefaultAsync<User>(command);
    }
}
