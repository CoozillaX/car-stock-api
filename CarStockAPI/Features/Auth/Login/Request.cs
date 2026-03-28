namespace CarStockAPI.Features.Auth.Login;

/// <summary>
/// Request model for the login endpoint,
/// containing the username and password provided by the user attempting to authenticate.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// The username of the user attempting to log in.
    /// This should match the username stored in the database for the user.
    /// </summary>
    public string Username { get; set; } = "";

    /// <summary>
    /// The raw password (or hashed by frontend) of the user attempting to log in.
    /// This should match the password hash stored in the database for the user.
    /// </summary>
    public string Password { get; set; } = "";
}
