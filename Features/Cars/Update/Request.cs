using FastEndpoints;

namespace car_stock_api.Features.Cars.Update;

/// <summary>
/// Request model for updating an existing car in the authenticated user's car stock.
/// </summary>
public class UpdateCarRequest
{
    /// <summary>
    /// Unique identifier for the car.
    /// </summary>
    [BindFrom("id")]
    public int Id { get; set; }

    /// <summary>
    /// The make of the car (e.g., Toyota, Honda).
    /// </summary>
    public string? Make { get; set; }

    /// <summary>
    /// The model of the car (e.g., Camry, Civic).
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// The year the car was manufactured.
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// The quantity of this car available in stock.
    /// </summary>
    public int? Stock { get; set; }
}
