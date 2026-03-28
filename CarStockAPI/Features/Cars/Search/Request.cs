namespace CarStockAPI.Features.Cars.Search;

/// <summary>
/// Request model for searching cars associated with the authenticated user.
/// </summary>
public class SearchCarRequest
{
    /// <summary>
    /// The make of the car (e.g., Toyota, Honda).
    /// </summary>
    public string? Make { get; set; }

    /// <summary>
    /// The model of the car (e.g., Camry, Civic).
    /// </summary>
    public string? Model { get; set; }
}
