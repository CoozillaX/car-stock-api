using Dapper;
using car_stock_api.Domain.Entities;
using car_stock_api.Infrastructure.Database;

namespace car_stock_api.Infrastructure.Repositories;

/// <summary>
/// Repository class for managing Car entities in the database.
/// </summary>
/// <param name="dbConnectionFactory">
/// The factory for creating database connections.
/// </param>
public class CarRepository(DbConnectionFactory dbConnectionFactory)
{
    private readonly DbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    /// <summary>
    /// Creates a new car in the database and returns the created car with its assigned Id.
    /// </summary>
    /// <param name="car">The car to create.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The created car with Id.</returns>
    /// <exception cref="Microsoft.Data.Sqlite.SqliteException">
    /// Thrown when a car with the same UserId, Make, Model and Year already exists.
    /// </exception>
    public async Task<Car> CreateAsync(Car car, CancellationToken ct = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var command = new CommandDefinition(
            """
            INSERT INTO Cars (UserId, Make, Model, Year, Stock) 
            VALUES (@UserId, @Make, @Model, @Year, @Stock);
            SELECT last_insert_rowid();
            """,
            car,
            cancellationToken: ct
        );

        var id = await connection.ExecuteScalarAsync<int>(command);
        car.Id = id;
        return car;
    }

    /// <summary>
    /// Removes a car from the database by its Id.
    /// Returns true if the car was successfully removed, false otherwise.
    /// </summary>
    /// <param name="id">The Id of the car to remove.</param>
    /// <param name="userId">The Id of the user who owns the car to remove.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>True if the car was successfully removed, false otherwise.</returns>
    public async Task<bool> RemoveAsync(int id, int userId, CancellationToken ct = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var command = new CommandDefinition(
            "DELETE FROM Cars WHERE Id = @Id AND UserId = @UserId;",
            new { Id = id, UserId = userId },
            cancellationToken: ct
        );

        var rows = await connection.ExecuteAsync(command);
        return rows > 0;
    }

    /// <summary>
    /// Updates an existing car in the database.
    /// Returns true if the car was successfully updated, false otherwise.
    /// </summary>
    /// <param name="id">The Id of the car to update.</param>
    /// <param name="userId">The Id of the user who owns the car to update.</param>
    /// <param name="make">The new make of the car, or null to keep the existing make.</param>
    /// <param name="model">The new model of the car, or null to keep the existing model.</param>
    /// <param name="year">The new year of the car, or null to keep the existing year.</param>
    /// <param name="stock">The new stock of the car, or null to keep the existing stock.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>True if the car was successfully updated, false otherwise.</returns>
    public async Task<bool> UpdateAsync(
        int id,
        int userId,
        string? make,
        string? model,
        int? year,
        int? stock,
        CancellationToken ct)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var command = new CommandDefinition(
            """
            UPDATE Cars 
            SET Make = COALESCE(@Make, Make), 
                Model = COALESCE(@Model, Model), 
                Year = COALESCE(@Year, Year), 
                Stock = COALESCE(@Stock, Stock)
            WHERE Id = @Id AND UserId = @UserId;
            """,
            new { Id = id, UserId = userId, Make = make, Model = model, Year = year, Stock = stock },
            cancellationToken: ct
        );

        var rows = await connection.ExecuteAsync(command);
        return rows > 0;
    }

    /// <summary>
    /// Retrieves a car from the database by its Id. Returns null if no car with the given Id exists.
    /// </summary>
    /// <param name="id">The Id of the car to retrieve.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The car with the specified Id, or null if not found.</returns>
    public async Task<Car?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var command = new CommandDefinition(
            "SELECT * FROM Cars WHERE Id = @Id;",
            new { Id = id },
            cancellationToken: ct
        );

        return await connection.QuerySingleOrDefaultAsync<Car>(command);
    }

    /// <summary>
    /// Retrieves all cars from the database that belong to a specific user, identified by their UserId.
    /// </summary>
    /// <param name="userId">The Id of the user whose cars to retrieve.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The collection of cars belonging to the specified user.</returns>
    public async Task<IEnumerable<Car>> GetByUserIdAsync(int userId, CancellationToken ct = default)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var command = new CommandDefinition(
            "SELECT * FROM Cars WHERE UserId = @UserId;",
            new { UserId = userId },
            cancellationToken: ct
        );

        return await connection.QueryAsync<Car>(command);
    }

    /// <summary>
    /// Retrieves cars from the database that match the given make and model for a specific user.
    /// </summary>
    /// <param name="userId">The Id of the user whose cars to search.</param>
    /// <param name="make">The make of the cars to search for.</param>
    /// <param name="model">The model of the cars to search for.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The collection of cars matching the make and model for the specified user.</returns>
    public async Task<IEnumerable<Car>> GetByMakeModelAsync(
        int userId,
        string make,
        string model,
        CancellationToken ct = default
    )
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var command = new CommandDefinition(
            """
            SELECT * FROM Cars 
            WHERE UserId = @UserId AND Make = @Make AND Model = @Model;
            """,
            new { UserId = userId, Make = make, Model = model },
            cancellationToken: ct
        );

        return await connection.QueryAsync<Car>(command);
    }
}
