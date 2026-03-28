using FastEndpoints;
using FastEndpoints.Security;
using CarStockAPI.Infrastructure.Repositories;

namespace CarStockAPI.Features.Auth.Login;

/// <summary>
/// Login endpoint that authenticates a user and returns a JWT token if the credentials are valid.
/// </summary>
/// <param name="repo">The user repository for accessing user data.</param>
/// <param name="config">The application configuration for accessing JWT settings.</param>
public class LoginEndpoint(UserRepository repo, IConfiguration config) : Endpoint<LoginRequest, LoginResponse>
{
    private readonly string _key =
        config["Jwt:SigningKey"] ??
        throw new InvalidOperationException("JWT signing key is not configured.");
    
    /// <summary>
    /// Configures the endpoint route.
    /// </summary>
    public override void Configure()
    {
        Post("/auth/login");
        RoutePrefixOverride("");

        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Authenticate user and return JWT token";
            s.Description = "Validates username and password, then returns a JWT token for authenticated requests.";
        });
        Description(s =>
        {
            s.Produces<LoginResponse>(200, "application/json");
            s.Produces(401);
        });
    }

    /// <summary>
    /// Handles the incoming request.
    /// </summary>
    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        // Get the user by username
        var user = await repo.GetByUsernameAsync(req.Username, ct);

        // If the user doesn't exist or the password is incorrect, return 401 Unauthorized
        if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        // Create a JWT token for the authenticated user
        var token = JwtBearer.CreateToken(o =>
        {
            o.SigningKey = _key;
            o.Issuer = "car-stock-api";
            o.ExpireAt = DateTime.UtcNow.AddHours(1);
            o.User["sub"] = user.Id.ToString();
        });

        await Send.OkAsync(new LoginResponse { Token = token }, ct);
    }
}
