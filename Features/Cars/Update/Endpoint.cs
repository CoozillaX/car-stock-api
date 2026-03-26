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
    public override void Configure()
    {
        Patch("/cars/{id}");
    }

    public override async Task HandleAsync(UpdateCarRequest req, CancellationToken ct)
    {
        var user = await GetUserAsync(ct);

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

        if (!success)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.NoContentAsync(ct);
    }
}
