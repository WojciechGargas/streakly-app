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
        Assert.NotNull(activity.UpdatedAt);
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
        user.MarkActivityAsIncompleted(activity.Id);
        
        //Assert
        Assert.False(activity.Completed);
        Assert.NotNull(activity.UpdatedAt);
    }

    [Fact]
    public void MarkActivityAsIncompleted_WhenActivityDoesNotExist_ThrowsActivityNotFoundException()
    {
        //Arrange
        var user = CreateTestUser();
        
        //Act
        var exception = Record.Exception(() => user.MarkActivityAsIncompleted(Guid.NewGuid()));
        
        //Assert
        Assert.IsType<ActivityNotFoundException>(exception);
    }

    [Fact]
    public void ChangeActivityName_WhenActivityExists_UpdatesName()
    {
        //Arrange
        var user = CreateTestUser();
        var activity = CreateTestActivity(user);
        user.AddActivity(activity);
        
        //Act
        user.ChangeActivityName(activity.Id, "updated_name");
        
        //Assert
        
        Assert.Equal("updated_name", activity.Name);
        Assert.NotNull(activity.UpdatedAt);
    }

    [Fact]
    public void ChangeActivityName_WhenActivityDoesNotExist_ThrowsActivityNotFoundException()
    {
        //Arrange
        var user = CreateTestUser();
        
        //Act
        var exception = Record.Exception(() => user.ChangeActivityName(Guid.NewGuid(), "updated_name"));
        
        //Assert
        Assert.IsType<ActivityNotFoundException>(exception);
    }

    [Fact]
    public void ChangeActivityDescription_WhenActivityExists_UpdatesDescription()
    {
        //Arrange
        var user = CreateTestUser();
        var activity = CreateTestActivity(user);
        user.AddActivity(activity);
        
        //Act
        user.ChangeActivityDescription(activity.Id, "updated_description");
        
        //Assert
        Assert.Equal("updated_description", activity.Description);
        Assert.NotNull(activity.UpdatedAt);
    }
    [Fact]
    public void ChangeActivityDescription_WhenActivityDoesNotExist_ThrowsActivityNotFoundException()
    {
        //Arrange
        var user = CreateTestUser();
        
        //Act
        var exception = Record.Exception(() => user.ChangeActivityDescription(Guid.NewGuid(), "updated_description"));
        
        //Assert
        Assert.IsType<ActivityNotFoundException>(exception);
    }

    [Fact]
    public void MarkAsLoggedIn_WhenCalled_UpdatesLastLoggedAtUtc()
    {
        //Arrange
        var user = CreateTestUser();
        var loggedAt = DateTime.UtcNow;
        
        //Act
        user.MarkAsLoggedIn(loggedAt);
        
        //Assert
        Assert.Equal(loggedAt, user.LastLoggedAtUtc);
    }

    [Fact]
    public void ChangeUsername_WhenValueIsValid_UpdatesUsername()
    {
        //Arrange
        var user = CreateTestUser();
        
        //Act
        user.ChangeUsername("updated_username");
        
        //Assert
        Assert.Equal("updated_username", user.Username);
    }
    
    [Fact]
    public void ChangeFullname_WhenValueIsValid_UpdatesFullname()
    {
        //Arrange
        var user = CreateTestUser();
        
        //Act
        user.ChangeFullName("updated_fullname");
        
        //Assert
        Assert.Equal("updated_fullname", user.FullName);
    }

    [Fact]
    public void ChangeEmail_WhenValueIsValid_UpdatesEmail()
    {
        //Arrange
        var user = CreateTestUser();
        
        //Act
        user.ChangeEmail("updatedemail@updated.com");
        
        //Assert
        Assert.Equal("updatedemail@updated.com", user.Email);
    }

    [Fact]
    public void ChangePassword_WhenValueIsValid_UpdatesPassword()
    {
        //Arrange
        var user = CreateTestUser();
        
        //Act
        user.ChangePassword("updated_password");
        
        //Assert
        Assert.Equal("updated_password", user.Password);
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