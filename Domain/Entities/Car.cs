namespace car_stock_api.Domain.Entities;

/// <summary>
/// Represents a car in the inventory.
/// </summary>
public class Car
{
    /// <summary>
    /// Unique identifier for the car. This is the primary key in the database.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Foreign key referencing the user who owns this car.
    /// This establishes a relationship between the Car and User entities.
    /// </summary>
    public required int UserId { get; set; }

    /// <summary>
    /// The make of the car (e.g., Toyota, Honda). 
    /// </summary>
    public required string Make { get; set; } = "";

    /// <summary>
    /// The model of the car (e.g., Camry, Civic). 
    /// </summary>
    public required string Model { get; set; } = "";

    /// <summary>
    /// The year the car was manufactured. 
    /// </summary>
    public int Year { get; set; }
    
    /// <summary>
    /// The quantity of this car available in stock. 
    /// </summary>
    public int Stock { get; set; }
}
