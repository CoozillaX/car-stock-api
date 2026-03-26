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
    public override void Configure()
    {
        Delete("/cars/{id}");
    }

    public override async Task HandleAsync(DeleteCarRequest req, CancellationToken ct)
    {
        var user = await GetUserAsync(ct);

        var car = await carRepo.GetByIdAsync(req.Id, ct);

        if (car is null || car.UserId != user.Id)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        var success = await carRepo.RemoveAsync(car.Id, user.Id, ct);
        if (!success)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.NoContentAsync(ct);
    }
}
