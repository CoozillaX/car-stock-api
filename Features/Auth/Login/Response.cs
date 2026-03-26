namespace car_stock_api.Features.Auth.Login;

/// <summary>
/// Response model for the login endpoint,
/// containing the JWT token issued to the user upon successful authentication.
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// The JWT token issued to the user upon successful authentication.
    /// This token should be included in the Authorization header of subsequent requests to protected endpoints.
    /// </summary>
    public string Token { get; set; } = "";
}
