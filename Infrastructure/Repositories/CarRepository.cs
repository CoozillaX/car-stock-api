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
    /// <returns>The created car with Id.</returns>
    /// <exception cref="Microsoft.Data.Sqlite.SqliteException">
    /// Thrown when a car with the same UserId, Make, Model and Year already exists.
    /// </exception>
    public async Task<Car> CreateAsync(Car car)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var id = await connection.ExecuteScalarAsync<int>(
            @"
            INSERT INTO Cars (UserId, Make, Model, Year, Stock) 
            VALUES (@UserId, @Make, @Model, @Year, @Stock);
            SELECT last_insert_rowid();
            ",
            car
        );
        car.Id = id;
        return car;
    }

    /// <summary>
    /// Removes a car from the database by its Id.
    /// Returns true if the car was successfully removed, false otherwise.
    /// </summary>
    /// <param name="id">The Id of the car to remove.</param>
    /// <returns>True if the car was successfully removed, false otherwise.</returns>
    public async Task<bool> RemoveAsync(int id)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(
            @"DELETE FROM Cars WHERE Id = @Id;",
            new { Id = id }
        );
        return rows > 0;
    }

    /// <summary>
    /// Updates an existing car in the database.
    /// Returns true if the car was successfully updated, false otherwise.
    /// </summary>
    /// <param name="car">The car to update.</param>
    /// <returns>True if the car was successfully updated, false otherwise.</returns>
    public async Task<bool> UpdateAsync(Car car)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(
            @"
            UPDATE Cars 
            SET UserId = @UserId, Make = @Make, Model = @Model, Year = @Year, Stock = @Stock
            WHERE Id = @Id;
            ",
            car
        );
        return rows > 0;
    }

    /// <summary>
    /// Retrieves a car from the database by its Id. Returns null if no car with the given Id exists.
    /// </summary>
    /// <param name="id">The Id of the car to retrieve.</param>
    /// <returns>The car with the specified Id, or null if not found.</returns>
    public async Task<Car?> GetByIdAsync(int id)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Car>(
            @"SELECT * FROM Cars WHERE Id = @Id;",
            new { Id = id }
        );
    }

    /// <summary>
    /// Retrieves all cars from the database that belong to a specific user, identified by their UserId.
    /// </summary>
    /// <param name="userId">The Id of the user whose cars to retrieve.</param>
    /// <returns>The collection of cars belonging to the specified user.</returns>
    public async Task<IEnumerable<Car>> GetByUserIdAsync(int userId)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        return await connection.QueryAsync<Car>(
            @"SELECT * FROM Cars WHERE UserId = @UserId;",
            new { UserId = userId }
        );
    }

    /// <summary>
    /// Retrieves cars from the database that match the given make and model for a specific user.
    /// </summary>
    /// <param name="userId">The Id of the user whose cars to search.</param>
    /// <param name="make">The make of the cars to search for.</param>
    /// <param name="model">The model of the cars to search for.</param>
    /// <returns>The collection of cars matching the make and model for the specified user.</returns>
    public async Task<IEnumerable<Car>> GetByMakeModelAsync(int userId, string make, string model)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        return await connection.QueryAsync<Car>(
            @"
            SELECT * FROM Cars 
            WHERE UserId = @UserId AND Make = @Make AND Model = @Model;
            ",
            new { UserId = userId, Make = make, Model = model }
        );
    }
}
