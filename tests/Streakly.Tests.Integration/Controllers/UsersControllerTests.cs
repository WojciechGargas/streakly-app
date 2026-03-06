using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Streakly.Application.DTO;
using Streakly.Application.Security;
using Streakly.Core.Entities;
using Streakly.Infrastructure.DAL;
using Streakly.Tests.Integration.Infrastructure;
using Streakly.Tests.Integration.Shared;

namespace Streakly.Tests.Integration.Controllers;

public class UsersControllerTests(ApplicationWebFactory factory) : IClassFixture<ApplicationWebFactory>, IAsyncLifetime
{
    private readonly HttpClient _backend = factory.CreateClient();

    public Task InitializeAsync()
    {
        AuthTestHelper.ClearAuthorization(_backend);
        return factory.ResetStateAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetAllUsers_ReturnsUsersList()
    {
        //Arrange
        const int expectedUsersCount = 8;
        await ReplaceUsersAsync(expectedUsersCount);

        //Act
        var response = await _backend.GetAsync("/users/all");
        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserDto>>();

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(users);
        Assert.Equal(expectedUsersCount, users.Count());
    }

    [Fact]
    public async Task GetUser_ExistingUser_ReturnsUser()
    {
        //Arrange
        var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        //Act
        var response = await _backend.GetAsync($"/users/{userId}");
        var user = await response.Content.ReadFromJsonAsync<UserDto>();

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(user);
        Assert.Equal(userId, user.Id);
    }

    [Fact]
    public async Task GetUser_NonexistingUser_ReturnsBadRequest()
    {
        //Arrange
        var userId = Guid.Parse("99999999-9999-9999-9999-999999999999");

        //Act
        var response = await _backend.GetAsync($"/users/{userId}");
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(error);
        Assert.Equal("user_not_found", error.Code);
        Assert.Equal($"User with ID : '{userId}' was not found.", error.Reason);
    }

    [Fact]
    public async Task DeleteUser_ExistingUser_AsRegularUser_ReturnsForbidden()
    {
        //Arrange
        var userIdFromToken = Guid.Parse("22222222-2222-2222-2222-222222222222");
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, userIdFromToken, "User");
        var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        //Act
        var deleteResponse = await _backend.DeleteAsync($"/users/{userId}");

        //Assert
        Assert.Equal(HttpStatusCode.Forbidden, deleteResponse.StatusCode);
    }
    
    [Fact]
    public async Task DeleteUser_ExistingUser_AsAdmin_ReturnsNoContent()
    {
        //Arrange
        var userIdFromToken = Guid.Parse("11111111-1111-1111-1111-111111111111");
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, userIdFromToken, "Admin");
        var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        //Act
        var deleteResponse = await _backend.DeleteAsync($"/users/{userId}");
        
        //Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteUser_NonexistingUser_AsAdmin_ReturnsBadRequest()
    {
        //Arrange
        var userIdFromToken = Guid.Parse("11111111-1111-1111-1111-111111111111");
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, userIdFromToken, "Admin");
        var userId = Guid.Parse("99999999-9999-9999-9999-999999999999");

        //Act
        var response = await _backend.DeleteAsync($"/users/{userId}");
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(error);
        Assert.Equal("user_not_found", error.Code);
        Assert.Equal($"User with ID : '{userId}' was not found.", error.Reason);
    }

    private async Task ReplaceUsersAsync(int count)
    {
        await using var scope = factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<StreaklyDbContext>();
        var passwordManager = scope.ServiceProvider.GetRequiredService<IPasswordManager>();

        await db.Users.ExecuteDeleteAsync();

        var users = new List<User>(count);

        for (var i = 0; i < count; i++)
        {
            var suffix = Guid.NewGuid().ToString("N")[..8];
            users.Add(new User(
                Guid.NewGuid(),
                $"user_{suffix}@streakly.test",
                $"user_{suffix}",
                passwordManager.Secure("User123!"),
                $"Test User {i + 1}",
                UserRole.User,
                DateTime.UtcNow.AddMinutes(-i),
                new List<Activity>()));
        }

        await db.Users.AddRangeAsync(users);
        await db.SaveChangesAsync();
    }
}
