using System.Net;
using System.Net.Http.Json;
using Streakly.Application.DTO;
using Streakly.Tests.Integration.Infrastructure;
using Streakly.Tests.Integration.Shared;

namespace Streakly.Tests.Integration.Controllers;

public class UsersControllerTests(ApplicationWebFactory factory) : IClassFixture<ApplicationWebFactory>
{
    private readonly HttpClient _backend = factory.CreateClient();

    [Fact]
    public async Task GetAllUsers_ReturnsUsersList()
    {
        //Arrange
        
        //Act
        var response = await _backend.GetAsync($"/users/all" );
        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserDto>>();
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response);
        Assert.NotNull(users);
        Assert.Equal(5, users.Count());
    }
    
    [Fact]
    public async Task GetUser_ExisitingUser_ReturnsUser()
    {
        //Arrange
        var exisitingUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        
        //Act
        var response = await _backend.GetAsync($"/users/{exisitingUserId}" );
        var user = await response.Content.ReadFromJsonAsync<UserDto>();
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(user);
        Assert.Equal(exisitingUserId, user.Id);
    }
    
    [Fact]
    public async Task GetUser_NonexisitingUser_ReturnsNotFound()
    {
        //Arrange
        var nonexisitingUserId = Guid.Parse("99999999-9999-9999-9999-999999999999");
        
        //Act
        var response = await _backend.GetAsync($"/users/{nonexisitingUserId}" );
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        
        //Assert
        Assert.NotNull(error);
        Assert.Equal("user_not_found", error.Code);
        Assert.Equal($"User with ID : '{nonexisitingUserId}' was not found.", error.Reason);
    }
}