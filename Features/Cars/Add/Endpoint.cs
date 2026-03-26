using car_stock_api.Common;
using car_stock_api.Infrastructure.Repositories;
using car_stock_api.Features.Cars.Dtos;
using car_stock_api.Domain.Entities;

namespace car_stock_api.Features.Cars.Add;

/// <summary>
/// Endpoint for adding a new car to the authenticated user's inventory.
/// </summary>
/// <param name="userRepo">The user repository for accessing user data.</param>
/// <param name="carRepo">The car repository for accessing car data.</param>
public class AddCarsEndpoint(UserRepository userRepo, CarRepository carRepo)
    : AuthenticatedEndpoint<AddCarRequest, CarDto>(userRepo)
{
    public override void Configure()
    {
        Post("/cars");
    }

    public override async Task HandleAsync(AddCarRequest req, CancellationToken ct)
    {
        var user = await GetUserAsync(ct);

        try
        {
            var car = await carRepo.CreateAsync(new Car
            {
                UserId = user.Id,
                Make = req.Make,
                Model = req.Model,
                Year = req.Year,
                Stock = req.Stock
            }, ct);

            // Return the created car with its assigned Id
            await Send.OkAsync(new CarDto
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Stock = car.Stock
            }, ct);
        }
        catch (Microsoft.Data.Sqlite.SqliteException ex) when (ex.SqliteErrorCode == 19)
        {
            ThrowError("A car with the same make, model and year already exists.", 400);
        }
    }
}
