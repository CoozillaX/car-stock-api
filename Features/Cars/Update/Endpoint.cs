using FastEndpoints;
using car_stock_api.Common;
using car_stock_api.Infrastructure.Repositories;

namespace car_stock_api.Features.Cars.Update;

/// <summary>
/// Endpoint for updating a car in the authenticated user's inventory.
/// </summary>
/// <param name="userRepo">The user repository for accessing user data.</param>
/// <param name="carRepo">The car repository for accessing car data.</param>
public class UpdateCarsEndpoint(UserRepository userRepo, CarRepository carRepo)
    : AuthenticatedEndpoint<UpdateCarRequest, EmptyResponse>(userRepo)
{
    /// <summary>
    /// Configures the endpoint route.
    /// </summary>
    public override void Configure()
    {
        Patch("/cars/{id}");

        Summary(s =>
        {
            s.Summary = "Update a car in the inventory";
            s.Description = "Updates a car in the authenticated user's inventory. The request must include the unique identifier of the car to be updated and any fields to be modified (make, model, year, stock). Only the provided fields will be updated.";
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
    public override async Task HandleAsync(UpdateCarRequest req, CancellationToken ct)
    {
        var user = await GetUserAsync(ct);

        try
        {
            // Permission check is handled in the repository layer
            var success = await carRepo.UpdateAsync(
                id: req.Id,
                userId: user.Id,
                make: req.Make,
                model: req.Model,
                year: req.Year,
                stock: req.Stock,
                ct: ct
            );

            if (success)
            {
                await Send.NoContentAsync(ct);
                return;
            }
        }
        catch (Microsoft.Data.Sqlite.SqliteException ex) when (ex.SqliteErrorCode == 19)
        {
            ThrowError("A car with the same make, model and year already exists.", 400);
        }
        
        await Send.NotFoundAsync(ct);
    }
}
