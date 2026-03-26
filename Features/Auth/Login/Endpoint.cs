using FastEndpoints;
using FastEndpoints.Security;
using car_stock_api.Infrastructure.Repositories;

namespace car_stock_api.Features.Auth.Login;

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
        
    public override void Configure()
    {
        Post("/auth/login");
        RoutePrefixOverride("");

        AllowAnonymous();
    }

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
