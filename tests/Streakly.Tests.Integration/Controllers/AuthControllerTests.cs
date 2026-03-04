using System.Net;
using System.Net.Http.Json;
using Streakly.Application.DTO;
using Streakly.Tests.Integration.Infrastructure;

namespace Streakly.Tests.Integration.Controllers;

public class AuthControllerTests(ApplicationWebFactory factory) : IClassFixture<ApplicationWebFactory>
{
    private readonly HttpClient _backend = factory.CreateClient();

    [Fact]
    public async Task SignIn_WithCorrectCredentials_ReturnsJwtToken()
    {
        //Arrange
        var request = new
        {
            email = "anna@streakly.test",
            password = "User123!"
        };

        //Act
        var response = await _backend.PostAsJsonAsync("/auth/sign-in", request);
        var jwt = await response.Content.ReadFromJsonAsync<JwtDto>();

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(jwt);
        Assert.False(string.IsNullOrWhiteSpace(jwt!.AccessToken));
    }
    [Fact]
    public async Task SignIn_WithIncorrectCredentials_ReturnsBadRequestAndErrorPayload()
    {
        //Arrange
        var request = new
        {
            email = "anna@streakly.test",
            password = "xzxxxxxxxx"
        };
        
        //Act
        var response = await _backend.PostAsJsonAsync("/auth/sign-in", request);
        
        //Assert
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(error);
        Assert.Equal("invalid_credentials", error.Code);
        Assert.Equal("Invalid credentials", error.Reason);
    }
    
    private sealed record ErrorResponse(string Code, string Reason);
}
