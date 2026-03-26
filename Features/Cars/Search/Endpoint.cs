using car_stock_api.Common;
using car_stock_api.Infrastructure.Repositories;
using car_stock_api.Features.Cars.Dtos;

namespace car_stock_api.Features.Cars.Search;

/// <summary>
/// Endpoint for searching cars associated with the authenticated user.
/// </summary>
/// <param name="userRepo">The user repository for accessing user data.</param>
/// <param name="carRepo">The car repository for accessing car data.</param>
public class SearchCarsEndpoint(UserRepository userRepo, CarRepository carRepo)
    : AuthenticatedEndpoint<SearchCarRequest, IEnumerable<CarDto>>(userRepo)
{
    /// <summary>
    /// Configures the endpoint route.
    /// </summary>
    public override void Configure()
    {
        Get("/cars");

        Summary(s =>
        {
            s.Summary = "Search cars in the inventory";
            s.Description = "Searches for cars in the authenticated user's inventory based on optional make and model filters. If no filters are provided, returns all cars in the inventory.";
        });
        Description(s =>
        {
            s.Produces<IEnumerable<CarDto>>(200, "application/json");
        });
    }

    /// <summary>
    /// Handles the incoming request.
    /// </summary>
    public override async Task HandleAsync(SearchCarRequest req, CancellationToken ct)
    {
        var user = await GetUserAsync(ct);

        var cars = await carRepo.GetByMakeModelAsync(user.Id, req.Make, req.Model, ct);

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
