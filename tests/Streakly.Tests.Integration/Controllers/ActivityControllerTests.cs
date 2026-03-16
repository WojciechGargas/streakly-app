using System.Net;
using System.Net.Http.Json;
using Streakly.Application.Commands.ActivityCommands;
using Streakly.Application.DTO;
using Streakly.Tests.Integration.Infrastructure;
using Streakly.Tests.Integration.Shared;

namespace Streakly.Tests.Integration.Controllers;

public class ActivityControllerTests(ApplicationWebFactory factory) : IClassFixture<ApplicationWebFactory>, IAsyncLifetime
{
    private readonly HttpClient _backend = factory.CreateClient();

    public Task InitializeAsync()
    {
        AuthTestHelper.ClearAuthorization(_backend);
        return factory.ResetStateAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task AddActivity_WhenAuthenticatedAndCorrectData_ReturnsCreated()
    {
        //Arrange
        var userId = TestUserId;
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, userId);

        var request = CreateCorrectAddActivityRequest();

        //Act
        var postResponse = await _backend.PostAsJsonAsync("/activity/addActivity", request);

        //Assert
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
        Assert.NotNull(postResponse.Headers.Location);

        var getResponse = await _backend.GetAsync(postResponse.Headers.Location);
        var activity = await getResponse.Content.ReadFromJsonAsync<ActivityDto>();

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        Assert.NotNull(activity);
        Assert.Equal("Run 5 km", activity.Name);
        Assert.Equal("Cardio training", activity.Description);
        Assert.Equal(1, (int)activity.Type);
    }

    [Fact]
    public async Task AddActivity_WhenUnauthenticatedAndCorrectData_ReturnsUnauthorized()
    {
        //Arrange
        var userId = TestUserId;
        
        var request = CreateCorrectAddActivityRequest();
        
        //Act
        var postResponse = await _backend.PostAsJsonAsync("/activity/addActivity", request);
        
        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, postResponse.StatusCode);
    }
    
    [Fact]
    public async Task AddActivity_WhenAuthenticatedAndIncorrectData_ReturnsBadRequest()
    {
        //Arrange
        var userId = TestUserId;
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, userId);

        var request = CreateIncorrectAddActivityRequest();
        
        //Act
        var postResponse = await _backend.PostAsJsonAsync("/activity/addActivity", request);
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
    }
    
    [Fact]
    public async Task DeleteActivity_WhenAuthenticatedAndExistingActivityId_ReturnsNoContentAndDeletesActivity()
    {
        //Arrange
        var userId = TestUserId;
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, userId);

        var request = CreateCorrectAddActivityRequest();
        
        //Act
        var postResponse = await _backend.PostAsJsonAsync("/activity/addActivity", request);
        
        var location = postResponse.Headers.Location ?? throw new Exception("Missing Location header");
        var createdActivityId = Guid.Parse(location.Segments[^1]);
        
        var deleteRequest = new { id = createdActivityId };
        using var deleteMessage = new HttpRequestMessage(HttpMethod.Delete, "/activity/deleteActivity")
        {
            Content = JsonContent.Create(deleteRequest)
        };
        var deleteResponse = await _backend.SendAsync(deleteMessage);
        
        //Assert
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task ChangeActivityName_WhenAuthenticatedAndIncorrectData_ReturnsBadRequest()
    {
        //Arrange
        var userId = TestUserId;
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, userId);
        
        var addActivityRequest = CreateCorrectAddActivityRequest();
        var postResponse = await _backend.PostAsJsonAsync("/activity/addActivity", addActivityRequest);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
        
        var location = postResponse.Headers.Location ?? throw new Exception("Missing Location header");
        var createdActivityId = Guid.Parse(location.Segments[^1]);
        var changeActivityNameRequest = CreateInorrectChangeActivityNameRequest(createdActivityId);
        
        //Act
        var patchResponse = await _backend.PatchAsJsonAsync("/activity/changeActivityName", changeActivityNameRequest);
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, patchResponse.StatusCode);
    }

    [Fact]
    public async Task MarkActivityAsCompleted_WhenAuthenticated_ReturnsNoContent_AndSetsCompletedFlag()
    {
        //Arrange
        var userId = TestUserId;
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, userId);
        
        var addActivityRequest = CreateCorrectAddActivityRequest();
        var postResponse = await _backend.PostAsJsonAsync("/activity/addActivity", addActivityRequest);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
        Assert.NotNull(postResponse.Headers.Location);
        
        var createdActivityId = Guid.Parse(
            postResponse.Headers.Location!.Segments.Last(s => !string.IsNullOrWhiteSpace(s)).TrimEnd('/'));
        var markActivityAsCompletedRequest = new MarkActivityAsCompletedRequest(createdActivityId);
        
        //Act
        var patchResponse = await _backend.PatchAsJsonAsync("/activity/markAsCompleted",
            markActivityAsCompletedRequest);
        
        //Assert
        Assert.Equal(HttpStatusCode.NoContent, patchResponse.StatusCode);
        
        var getResponse = await _backend.GetAsync(postResponse.Headers.Location);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var updatedActivity = await getResponse.Content.ReadFromJsonAsync<ActivityDto>();
        Assert.NotNull(updatedActivity);
        Assert.True(updatedActivity.Completed); 
    }

    private static readonly Guid TestUserId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static object CreateCorrectAddActivityRequest()
    {
        string name = "Run 5 km";
        string description = "Cardio training";

        return new
        {
            name = new
            {
                value = name
            },
            description = new
            {
                value = description
            },
            startDate = DateTime.Parse("2026-03-06T10:00:00Z"),
            endDate = DateTime.Parse("2026-03-20T10:00:00Z"),
            frequencyType = 1
        };
    }
    
    private static object CreateIncorrectAddActivityRequest()
    {
        string name = "";
        string description = "";

        return new
        {
            name = new
            {
                value = name
            },
            description = new
            {
                value = description
            },
            startDate = DateTime.Parse("2026-03-06T10:00:00Z"),
            endDate = DateTime.Parse("2026-03-20T10:00:00Z"),
            frequencyType = 1
        };
    }

    private static object CreateCorrectChangeActivityNameRequest(Guid id)
    {
        return new
        {
            Id = id,
            Description = "updated_description"
        };
    }
    
    private static object CreateInorrectChangeActivityNameRequest(Guid id)
    {
        return new
        {
            Id = id,
            Description = ""
        };
    }
}
