namespace car_stock_api.Domain.Entities;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User
{
    /// <summary>
    /// Unique identifier for the user. This is the primary key in the database.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The username of the user.
    /// </summary>
    public required string Username { get; set; } = "";

    /// <summary>
    /// The hash of the user's password.
    /// </summary>
    public required string PasswordHash { get; set; } = "";
}
