using FastEndpoints;
using car_stock_api.Infrastructure.Repositories;
using car_stock_api.Domain.Entities;

namespace car_stock_api.Common;

/// <summary>
/// Base class for authenticated endpoints that require a valid JWT token to access.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned by the endpoint.</typeparam>
/// <param name="userRepo">The user repository for accessing user data.</param>
public abstract class AuthenticatedEndpoint<TResponse>(
    UserRepository userRepo)
    : EndpointWithoutRequest<TResponse>
{
    /// <summary>
    /// Retrieves the authenticated user from the JWT token in the request.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The authenticated user.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authenticated or not found.</exception>
    protected async Task<User> GetUserAsync(CancellationToken ct = default)
    {
        if (!int.TryParse(User.FindFirst("sub")?.Value, out var userId))
            throw new UnauthorizedAccessException("Invalid token");

        var user = await userRepo.GetByIdAsync(userId, ct);

        return user ?? throw new UnauthorizedAccessException("User not found");
    }
}

/// <summary>
/// Base class for authenticated endpoints that require a valid JWT token to access and accept a request body.
/// </summary>
/// <typeparam name="TRequest">The type of the request body.</typeparam>
/// <typeparam name="TResponse">The type of the response returned by the endpoint.</typeparam>
/// <param name="userRepo">The user repository for accessing user data.</param>
public abstract class AuthenticatedEndpoint<TRequest, TResponse>(
    UserRepository userRepo)
    : Endpoint<TRequest, TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Retrieves the authenticated user from the JWT token in the request.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The authenticated user.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authenticated or not found.</exception>
    protected async Task<User> GetUserAsync(CancellationToken ct = default)
    {
        if (!int.TryParse(User.FindFirst("sub")?.Value, out var userId))
            throw new UnauthorizedAccessException("Invalid token");

        var user = await userRepo.GetByIdAsync(userId, ct);

        return user ?? throw new UnauthorizedAccessException("User not found");
    }
}
