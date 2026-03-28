namespace CarStockAPI.Features.Cars.Delete;

/// <summary>
/// Request model for deleting a car from the authenticated user's car stock.
/// </summary>
public class DeleteCarRequest
{
    /// <summary>
    /// Unique identifier for the car.
    /// </summary>
    public int Id { get; set; }
}
