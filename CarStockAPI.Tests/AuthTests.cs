using Microsoft.AspNetCore.Mvc.Testing;

namespace CarStockAPI.Tests;

/// <summary>
/// Tests for authentication-related functionality, such as login and token generation.
/// </summary>
/// <param name="factory">The WebApplicationFactory used to create an HttpClient for testing.</param>
public class AuthTests(WebApplicationFactory<Program> factory) : TestBase(factory)
{
    [Fact]
    public async Task Login_Should_Return_Token()
    {
        var token = await GetToken();

        Assert.False(string.IsNullOrEmpty(token));
    }
}
