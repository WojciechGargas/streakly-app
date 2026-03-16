using Streakly.Core.Entities;
using Streakly.Core.Exceptions;
using Streakly.Core.ValueObjects;

namespace Streakly.Tests.Unit.Core.Entities;

public class UserTests
{
    [Fact]
    public void AddActivity_WhenActivityIsValid_AddsActivity()
    {
        // Arrange
        var user =  CreateTestUser();
        var activity = CreateTestActivity(user);
        
        //Act
        user.AddActivity(activity);
        
        //Assert
        Assert.Single(user.Activities);
        Assert.Equal(activity.Id, user.Activities[0].Id);
    }

    [Fact]
    public void DeleteActivity_WhenActivityExists_RemovesActivity()
    {
        // Arrange
        var user = CreateTestUser();
        var activity = CreateTestActivity(user);
        user.AddActivity(activity);
        
        //Act
        user.DeleteActivity(activity.Id);
        
        //Assert
        Assert.Empty(user.Activities);
    }

    [Fact]
    public void DeleteActivity_WhenActivityDoesNotExist_ThrowsActivityNotFoundException()
    {
        //Arrange
        var user = CreateTestUser();
        var nonExistingActivityId = Guid.NewGuid();
        
        //Act
        var exception = Record.Exception(() => user.DeleteActivity(nonExistingActivityId));
        
        //Assert
        Assert.IsType<ActivityNotFoundException>(exception);
    }

    [Fact]
    public void MarkActivityAsCompleted_WhenActivityExists_SetsCompletedTrue()
    {
        //Arrange
        var user = CreateTestUser();
        var activity = CreateTestActivity(user);
        user.AddActivity(activity);
        
        //Act
        user.MarkActivityAsCompleted(activity.Id);
        
        //Assert
        Assert.True(activity.Completed);
    }

    [Fact]
    public void MarkActivityAsCompleted_WhenActivityDoesNotExist_ThrowsActivityNotFoundException()
    {
        //Arrange
        var user = CreateTestUser();
        
        //Act
        var exception = Record.Exception(() => user.MarkActivityAsCompleted(Guid.NewGuid()));
        
        //Assert
        Assert.IsType<ActivityNotFoundException>(exception);
    }

    [Fact]
    public void MarkActivityAsIncompleted_WhenActivityExists_SetsCompletedFalse()
    {
        //Arrange
        var user = CreateTestUser();
        var activity = CreateTestActivity(user);
        user.AddActivity(activity);
        user.MarkActivityAsCompleted(activity.Id);
        
        //Act
        Assert.False(activity.Completed);
        
    }

    private User CreateTestUser()
    {
        var now = DateTime.UtcNow; 
        return new User(
            new UserId(Guid.NewGuid()),
            new Email("test@streakly.test"),
            new Username("testuser"),
            new Password("User123!"),
            new FullName("Test User"),
            UserRole.User,
            DateTime.UtcNow,
            new List<Activity>());
    }
    
    private Activity CreateTestActivity(User user)
    {
        var now = DateTime.UtcNow; 
        return Activity.Create(
            user.UserId,
            new ActivityName("Run 5 km"),
            new ActivityDescription("Morning running"),
            now,
            now,
            null,
            ActivityFrequencyType.Daily);
    }
}