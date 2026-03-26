using FastEndpoints;
using car_stock_api.Common;
using car_stock_api.Infrastructure.Repositories;

namespace car_stock_api.Features.Cars.Delete;

/// <summary>
/// Endpoint for deleting a car from the authenticated user's inventory.
/// </summary>
/// <param name="userRepo">The user repository for accessing user data.</param>
/// <param name="carRepo">The car repository for accessing car data.</param>
public class DeleteCarsEndpoint(UserRepository userRepo, CarRepository carRepo)
    : AuthenticatedEndpoint<DeleteCarRequest, EmptyResponse>(userRepo)
{
    /// <summary>
    /// Configures the endpoint route.
    /// </summary>
    public override void Configure()
    {
        Delete("/cars/{id}");

        Summary(s =>
        {
            s.Summary = "Delete a car from the inventory";
            s.Description = "Deletes a car from the authenticated user's inventory. The request must include the unique identifier of the car to be deleted.";
        });
        Description(s =>
        {
            s.Produces(204);
            s.Produces(404);
        });
    }

    /// <summary>
    /// Handles the incoming request.
    /// </summary>
    public override async Task HandleAsync(DeleteCarRequest req, CancellationToken ct)
    {
        var user = await GetUserAsync(ct);

        // Permission check is handled in the repository layer
        var success = await carRepo.RemoveAsync(req.Id, user.Id, ct);
        if (!success)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.NoContentAsync(ct);
    }
}
