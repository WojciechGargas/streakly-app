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
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("invalid_credentials", error.Code);
        Assert.Equal("Invalid credentials", error.Reason);
    }

    [Fact]
    public async Task SignUp_WithValidData_ReturnsCreated_AndUserCanSignIn()
    {
        //Arrange
        var request = new
        {
            Email = "SignupTest@test.com",
            UserName = "SignupTest",
            Password = "User123!",
            FullName = "SignupTestFullName",
            Role = "User"
        };
        
        //Act
        await _backend.PostAsJsonAsync("/auth/", request);
        var login = new
        {
            email = "SignupTest@test.com",
            password = "User123!"
        };
        var response = await _backend.PostAsJsonAsync("/auth/sign-in", login);
        var jwt = await response.Content.ReadFromJsonAsync<JwtDto>();
        
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(jwt);
        Assert.False(string.IsNullOrWhiteSpace(jwt!.AccessToken));
    }

    [Fact]
    public async Task SignUp_WithDuplicateEmail_ReturnsBadRequest()
    {
        //Arrange
        var duplicateEmail = "anna@streakly.test";
        var request = new
        {
            Email = duplicateEmail,
            UserName = "SignupTest",
            Password = "User123!",
            FullName = "SignupTestFullName",
            Role = "User"
        };
        
        //Act
        var response = await _backend.PostAsJsonAsync("/auth/", request);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        
        //Assert
        Assert.NotNull(error);
        Assert.Equal("email_already_in_use", error.Code);
        Assert.Equal($"Email '{duplicateEmail}' is already in use", error.Reason);
    }
    
    [Fact]
    public async Task SignUp_WithInvalidEmail_ReturnsBadRequest()
    {
        //Arrange
        var invalidEmail = "xxx";
        var request = new
        {
            Email = invalidEmail,
            UserName = "SignupTest",
            Password = "User123!",
            FullName = "SignupTestFullName",
            Role = "User"
        };
        
        //Act
        var response = await _backend.PostAsJsonAsync("/auth/", request);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        
        //Assert
        Assert.NotNull(error);
        Assert.Equal("invalid_email", error.Code);
        Assert.Equal($"Email: {invalidEmail} is not a valid e-mail address.", error.Reason);
    }
    
    [Fact]
    public async Task SignUp_WithDuplicateUsername_ReturnsBadRequest()
    {
        //Arrange
        var duplicateUsername = "anna";
        var request = new
        {
            Email = "SignupTest2@test.com",
            UserName = duplicateUsername,
            Password = "User123!",
            FullName = "SignupTestFullName",
            Role = "User"
        };
        
        //Act
        var response = await _backend.PostAsJsonAsync("/auth/", request);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        
        //Assert
        Assert.NotNull(error);
        Assert.Equal("username_already_in_use", error.Code);
        Assert.Equal($"Username '{duplicateUsername}' is already in use", error.Reason);
    }

    [Fact]
    public async Task SignUp_WithTooShortPassword_ReturnsBadRequest()
    {
        //Arrange
        var tooShortPassword = "xxx";
        var request = new
        {
            Email = "SignupTest3@test.com",
            UserName = "sample",
            Password = tooShortPassword,
            FullName = "SignupTestFullName",
            Role = "User"
        };
        
        //Act
        var response = await _backend.PostAsJsonAsync("/auth/", request);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        
        //Assert
        Assert.NotNull(error);
        Assert.Equal("invalid_password", error.Code);
        Assert.Equal($"Provided password '{tooShortPassword}' is not a valid password.", error.Reason);
    }
    
    private sealed record ErrorResponse(string Code, string Reason);
}
