using car_stock_api.Common;
using car_stock_api.Infrastructure.Repositories;
using car_stock_api.Features.Cars.Dtos;

namespace car_stock_api.Features.Cars.List;

/// <summary>
/// Endpoint for listing all cars associated with the authenticated user.
/// </summary>
/// <param name="userRepo">The user repository for accessing user data.</param>
/// <param name="carRepo">The car repository for accessing car data.</param>
public class ListCarsEndpoint(UserRepository userRepo, CarRepository carRepo)
    : AuthenticatedEndpoint<IEnumerable<CarDto>>(userRepo)
{
    public override void Configure()
    {
        Get("/cars");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var user = await GetUserAsync(ct);

        var cars = await carRepo.GetByUserIdAsync(user.Id, ct);

        await Send.OkAsync(cars.Select(c => new CarDto
        {
            Id = c.Id,
            Make = c.Make,
            Model = c.Model,
            Year = c.Year,
            Stock = c.Stock
        }), ct);
    }
}
