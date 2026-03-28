namespace CarStockAPI.Features.Cars.Dtos;

/// <summary>
/// Data Transfer Object (DTO) for a car.
/// </summary>
public class CarDto
{
    /// <summary>
    /// Unique identifier for the car.
    /// </summary>
    public int Id { get; set; }

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
