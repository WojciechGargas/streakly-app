using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Streakly.Application.DTO;
using Streakly.Tests.Integration.Infrastructure;

namespace Streakly.Tests.Integration.Controllers;

public class ProfileControllerTests(ApplicationWebFactory factory) : IClassFixture<ApplicationWebFactory>, IAsyncLifetime
{
    private readonly HttpClient _backend = factory.CreateClient();

    public Task InitializeAsync()
    {
        _backend.DefaultRequestHeaders.Authorization = null;
        return factory.ResetStateAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetMe_WithoutToken_ReturnsUnauthorized()
    {
        //Act
        var response = await _backend.GetAsync("/profile/me");

        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMe_WithToken_ReturnsCurrentUser()
    {
        //Arrange
        var expectedUserId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        AuthenticateAs(expectedUserId);

        //Act
        var response = await _backend.GetAsync("/profile/me");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var user = await response.Content.ReadFromJsonAsync<UserDto>();

        //Assert
        Assert.NotNull(user);
        Assert.Equal(expectedUserId, user.Id);
        Assert.Equal("anna", user.Username);
    }

    private void AuthenticateAs(Guid userId, string role = "User")
    {
        var authSettings = GetAuthSettings();

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userId.ToString()),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: authSettings.Issuer,
            audience: authSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        _backend.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
    }

    private AuthSettings GetAuthSettings()
    {
        using var scope = factory.Services.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var issuer = configuration["auth:issuer"]
                     ?? throw new InvalidOperationException("Missing configuration key: auth:issuer");
        var audience = configuration["auth:audience"]
                       ?? throw new InvalidOperationException("Missing configuration key: auth:audience");
        var signingKey = configuration["auth:signingKey"]
                         ?? throw new InvalidOperationException("Missing configuration key: auth:signingKey");

        return new AuthSettings(issuer, audience, signingKey);
    }

    private sealed record AuthSettings(string Issuer, string Audience, string SigningKey);
}
