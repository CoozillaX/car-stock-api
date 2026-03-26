namespace car_stock_api.Features.Cars.Add;

/// <summary>
/// Request model for adding a new car to the authenticated user's car stock.
/// </summary>
public class AddCarRequest
{
    /// <summary>
    /// The make of the car (e.g., Toyota, Honda).
    /// </summary>
    public string Make { get; set; } = "";

    /// <summary>
    /// The model of the car (e.g., Camry, Civic).
    /// </summary>
    public string Model { get; set; } = "";

    /// <summary>
    /// The year the car was manufactured.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// The quantity of this car available in stock.
    /// </summary>
    public int Stock { get; set; }
}
