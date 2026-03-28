using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CarStockAPI.Tests;

/// <summary>
/// Base class for integration tests, providing common setup and utilities such as authentication.
/// </summary>
/// <param name="factory">The WebApplicationFactory used to create an HttpClient for testing.</param>
public abstract class TestBase(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient Client = factory.CreateClient();

    protected async Task<string> GetToken()
    {
        var response = await Client.PostAsJsonAsync("/auth/login", new
        {
            username = "user1",
            password = "password"
        });

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<LoginResponse>();

        return data!.Token;
    }

    protected async Task AuthorizeAsync()
    {
        var token = await GetToken();

        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    private class LoginResponse
    {
        public string Token { get; set; } = "";
    }
}
