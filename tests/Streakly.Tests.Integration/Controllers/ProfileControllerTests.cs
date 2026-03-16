using System.Net;
using System.Net.Http.Json;
using Streakly.Application.DTO;
using Streakly.Tests.Integration.Infrastructure;
using Streakly.Tests.Integration.Shared;

namespace Streakly.Tests.Integration.Controllers;

public class ProfileControllerTests(ApplicationWebFactory factory) : IClassFixture<ApplicationWebFactory>, IAsyncLifetime
{
    private readonly HttpClient _backend = factory.CreateClient();

    public Task InitializeAsync()
    {
        AuthTestHelper.ClearAuthorization(_backend);
        return factory.ResetStateAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetMe_WhenUnauthenticated_ReturnsUnauthorized()
    {
        //Act
        var response = await _backend.GetAsync("/profile/me");

        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMe_WhenAuthenticated_ReturnsCurrentUser()
    {
        //Arrange
        var expectedUserId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, expectedUserId);

        //Act
        var response = await _backend.GetAsync("/profile/me");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var user = await response.Content.ReadFromJsonAsync<UserDto>();

        //Assert
        Assert.NotNull(user);
        Assert.Equal(expectedUserId, user.Id);
        Assert.Equal("anna", user.Username);
    }

    [Fact]
    public async Task ChangeMyUsername_WhenUnauthenticated_ReturnsUnauthorized()
    {
        //Arrange
        var request = new
        {
            newUsername = "updated_username"
        };
        
        //Act
        var response = await _backend.PatchAsJsonAsync("/profile/me/changeUsername", request);
        
        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    [Fact]
    public async Task ChangeMyUsername_WhenAuthenticated_UpdatesUsername()
    {
        //Arrange
        var userId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, userId);

        var request = new
        {
            newUsername = "updated_username"
        };
        
        //Act
        var patchResponse = await _backend.PatchAsJsonAsync("/profile/me/changeUsername", request);
        var getResponse = await _backend.GetAsync("/profile/me");
        
        //Assert
        Assert.Equal(HttpStatusCode.NoContent, patchResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        
        var exisitingUser = await getResponse.Content.ReadFromJsonAsync<UserDto>();
        
        Assert.NotNull(exisitingUser);
        Assert.Equal(userId, exisitingUser.Id);
        Assert.Equal("updated_username", exisitingUser.Username);
    }
    
    [Fact]
    public async Task ChangeMyFullname_WhenUnauthenticated_ReturnsUnauthorized()
    {
        //Arrange
        var request = new
        {
            newFullname = "updated_fullname"
        };
        
        //Act
        var response = await _backend.PatchAsJsonAsync("/profile/me/changeFullname", request);
        
        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    [Fact]
    public async Task ChangeMyFullname_WhenAuthenticated_UpdatesFullname()
    {
        //Arrange
        var userId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, userId);

        var request = new
        {
            newFullname = "updated_fullname"
        };
        
        //Act
        var patchResponse = await _backend.PatchAsJsonAsync("/profile/me/changeFullname", request);
        var getResponse = await _backend.GetAsync("/profile/me");
        
        //Assert
        Assert.Equal(HttpStatusCode.NoContent, patchResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        
        var exisitingUser = await getResponse.Content.ReadFromJsonAsync<UserDto>();
        
        Assert.NotNull(exisitingUser);
        Assert.Equal(userId, exisitingUser.Id);
        Assert.Equal("updated_fullname", exisitingUser.FullName);
    }
    
    [Fact]
    public async Task ChangeMyPassword_WhenUnauthenticated_ReturnsUnauthorized()
    {
        //Arrange
        var request = new
        {
            newPassword = "11111111"
        };
        
        //Act
        var response = await _backend.PatchAsJsonAsync("/profile/me/changePassword", request);
        
        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    [Fact]
    public async Task ChangeMyPassword_WhenAuthenticated_UpdatesPassword()
    {
        //Arrange
        var newPassword = "11111111";
        var userId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, userId);
        
        var newPasswordRequest = new
        {
            oldPassword = "User123!",
            newPassword = newPassword
        };
        
        var signInWithNewPasswordRequest = new
        {
            email = "anna@streakly.test",
            password = newPassword
        };

        var signInWithOldPasswordRequest = new
        {
            email = "anna@streakly.test",
            password = "User123!"
        };
        
        //Act
        var patchResponse = await _backend.PatchAsJsonAsync("/profile/me/changePassword", newPasswordRequest);
        var signInWithUpdatedPasswordResponse = await _backend.PostAsJsonAsync("/auth/sign-in", signInWithNewPasswordRequest);
        var signInWithOldPasswordResponse = await _backend.PostAsJsonAsync("/auth/sign-in", signInWithOldPasswordRequest);
        
        //Assert
        Assert.Equal(HttpStatusCode.NoContent, patchResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, signInWithUpdatedPasswordResponse.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, signInWithOldPasswordResponse.StatusCode);
    }
    
    [Theory]
    [InlineData("1")]
    [InlineData("")]
    [InlineData("1111111111111111111111111111111111111111111111111111111" +
                "1111111111111111111111111111111111111111111111111111111111111111111" +
                "1111111111111111111111111111111111111111111111111111111111111111111")]
    public async Task ChangeMyPassword_WhenAuthenticatedAndIncorrectPassword_ReturnsBadRequest(string newPassword)
    {
        //Arrange
        var userId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, userId);
        
        var newPasswordRequest = new
        {
            oldPassword = "User123!",
            newPassword = newPassword
        };
        
        //Act
        var patchResponse = await _backend.PatchAsJsonAsync("/profile/me/changePassword", newPasswordRequest);
        var error = await patchResponse.Content.ReadFromJsonAsync<ErrorResponse>();
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, patchResponse.StatusCode);
        Assert.NotNull(error);
        Assert.Equal("invalid_password", error.Code);
        Assert.Equal($"Provided password '{newPassword}' is not a valid password.", error.Reason);
    }
    
    [Fact]
    public async Task ChangeMyEmail_WhenUnauthenticated_ReturnsUnauthorized()
    {
        //Arrange
        var request = new
        {
            newEmail = "updatedEmail@updated.test"
        };
        
        //Act
        var response = await _backend.PatchAsJsonAsync("/profile/me/changeEmail", request);
        
        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    [Fact]
    public async Task ChangeMyEmail_WhenAuthenticated_UpdatesEmail()
    {
        //Arrange
        var newEmail = "updatedEmail@updated.test";
        var userId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, userId);
        
        var newEmailRequest = new
        {
            newEmail = newEmail
        };
        
        var signInWithNewEmailRequest = new
        {
            email = newEmail,
            password = "User123!"
        };

        var signInWithOldEmailRequest = new
        {
            email = "anna@streakly.test",
            password = "User123!"
        };
        
        //Act
        var patchResponse = await _backend.PatchAsJsonAsync("/profile/me/changeEmail", newEmailRequest);
        var signInWithUpdatedEmailResponse = await _backend.PostAsJsonAsync("/auth/sign-in", signInWithNewEmailRequest);
        var signInWithOldEmailResponse = await _backend.PostAsJsonAsync("/auth/sign-in", signInWithOldEmailRequest);
        
        //Assert
        Assert.Equal(HttpStatusCode.NoContent, patchResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, signInWithUpdatedEmailResponse.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, signInWithOldEmailResponse.StatusCode);
    }
    
    [Fact]
    public async Task ChangeMyEmail_WhenAuthenticatedAndInvalidEmail_ReturnsBadRequest()
    {
        //Arrange
        var newEmail = "xxx";
        var userId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, userId);
        
        var newEmailRequest = new
        {
            newEmail = newEmail
        };
        
        //Act
        var patchResponse = await _backend.PatchAsJsonAsync("/profile/me/changeEmail", newEmailRequest);
        var error = await patchResponse.Content.ReadFromJsonAsync<ErrorResponse>();
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, patchResponse.StatusCode);
        Assert.NotNull(error);
        Assert.Equal("invalid_email", error.Code);
        Assert.Equal($"Email: {newEmail} is not a valid e-mail address.", error.Reason);
    }

    [Fact]
    public async Task ChangeMyEmail_WhenAuthenticatedAndAlreadyUsedEmail_ReturnsBadRequest()
    {
        //Arrange
        var newEmail = "admin@streakly.test";
        var userId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, userId);
        
        var newEmailRequest = new
        {
            newEmail = newEmail
        };
        
        //Act
        var patchResponse = await _backend.PatchAsJsonAsync("/profile/me/changeEmail", newEmailRequest);
        var error = await patchResponse.Content.ReadFromJsonAsync<ErrorResponse>();
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, patchResponse.StatusCode);
        Assert.NotNull(error);
        Assert.Equal("email_already_in_use", error.Code);
        Assert.Equal($"Email '{newEmail}' is already in use", error.Reason);
    }
    
    [Fact]
    public async Task ChangeMyEmail_WhenAuthenticatedAndSameEmail_ReturnsBadRequest()
    {
        //Arrange
        var newEmail = "anna@streakly.test";
        var userId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, userId);
        
        var newEmailRequest = new
        {
            newEmail = newEmail
        };
        
        //Act
        var patchResponse = await _backend.PatchAsJsonAsync("/profile/me/changeEmail", newEmailRequest);
        var error = await patchResponse.Content.ReadFromJsonAsync<ErrorResponse>();
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, patchResponse.StatusCode);
        Assert.NotNull(error);
        Assert.Equal("new_email_must_be_different", error.Code);
        Assert.Equal("New email must be different from the current email", error.Reason);
    }
}
