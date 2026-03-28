using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CarStockAPI.Tests;

/// <summary>
/// Tests for car-related operations, including creating, updating, searching, and deleting cars in the inventory.
/// These tests ensure that the car management features of the API work as expected and that permissions are correctly enforced.
/// </summary>
/// <param name="factory">The WebApplicationFactory used to create an HttpClient for testing.</param>
public class CarTests(WebApplicationFactory<Program> factory) : TestBase(factory)
{
    [Fact]
    public async Task Car_CRUD_Should_Work()
    {
        await AuthorizeAsync();

        var car = new CarDto(
            0,
            "TestMake-" + Guid.NewGuid(),
            "TestModel-" + Guid.NewGuid(),
            2020,
            10
        );

        // 1. create
        var createResponse = await Client.PostAsJsonAsync("/api/cars", car);

        createResponse.EnsureSuccessStatusCode();

        var createdCar = await createResponse.Content.ReadFromJsonAsync<CarDto>();

        Assert.NotNull(createdCar);
        Assert.Equal(car.Make, createdCar.Make);
        Assert.Equal(car.Model, createdCar.Model);
        Assert.Equal(car.Year, createdCar.Year);
        Assert.Equal(car.Stock, createdCar.Stock);

        car = createdCar;

        // 2. update
        var updatedStock = 20;
        var updateResponse = await Client.PatchAsJsonAsync(
            $"/api/cars/{car.Id}",
            new { stock = updatedStock });

        updateResponse.EnsureSuccessStatusCode();

        // 3. search
        var searchResponse = await Client.GetAsync($"/api/cars?make={car.Make}&model={car.Model}");

        searchResponse.EnsureSuccessStatusCode();

        var searchResults = await searchResponse.Content.ReadFromJsonAsync<List<CarDto>>();
        Assert.NotNull(searchResults);
        Assert.Contains(searchResults, c => c.Id == car.Id && c.Stock == updatedStock);

        // 4. delete
        var deleteResponse = await Client.DeleteAsync($"/api/cars/{car.Id}");
        deleteResponse.EnsureSuccessStatusCode();
    }

    private record CarDto(int Id, string Make, string Model, int Year, int Stock);
}
