using System.Net;
using System.Net.Http.Json;
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
    public async Task AddActivity_WithTokenAndCorrectData_ReturnsCreated()
    {
        //Arrange
        var userId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        AuthTestHelper.AuthenticateByJwt(_backend, factory.Services, userId);

        var request = new
        {
            name = new
            {
                value = "Run 5 km"
            },
            description = new
            {
                value = "Cardio training"
            },
            startDate = DateTime.Parse("2026-03-06T10:00:00Z"),
            endDate = DateTime.Parse("2026-03-20T10:00:00Z"),
            frequencyType = 1
        };

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
}
