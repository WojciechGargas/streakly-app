using Streakly.Core.Entities;
using Streakly.Core.ValueObjects;
using Streakly.Tests.Unit.Shared;

namespace Streakly.Tests.Unit.Core.Entities;

public class ActivityTests
{
    [Fact]
    public void MarkActivityAsCompleted_WhenCalled_SetsCompletedTrue()
    {
        //Arrange
        var activity = TestData.CreateActivity();
        
        //Act
        activity.MarkActivityAsCompleted();
        
        //Assert
        Assert.True(activity.Completed);
    }

    [Fact]
    public void MarkActivityAsCompleted_WhenCalled_UpdatesUpdatedAt()
    {
        //Arrange
        var activity = TestData.CreateActivity();
        
        //Act
        activity.MarkActivityAsCompleted();
        
        //Assert
        Assert.NotNull(activity.UpdatedAt);
    }

    [Fact]
    public void MarkActivityAsIncompleted_WhenCalled_SetsCompletedFalse()
    {
        //Arrange
        var activity = TestData.CreateActivity();
        activity.MarkActivityAsCompleted();
        
        //Act
        activity.MarkActivityAsIncompleted();
        
        //Assert
        Assert.False(activity.Completed);
    }

    [Fact]
    public void MarkActivityAsIncompleted_WhenCalled_UpdatesUpdatedAt()
    {
        //Arrange
        var activity = TestData.CreateActivity();
        
        //Act
        activity.MarkActivityAsIncompleted();
        
        //Assert
        Assert.NotNull(activity.UpdatedAt);
    }

    [Fact]
    public void ChangeName_WhenValueIsValid_UpdatesName()
    {
        //Arrange
        var activity = TestData.CreateActivity();
        
        //Act
        activity.ChangeName("updated_name");
        
        //Assert
        Assert.Equal("updated_name", activity.Name);
    }

    [Fact]
    public void ChangeName_WhenCalled_UpdatesUpdatedAt()
    {
        //Arrange
        var activity = TestData.CreateActivity();
        
        //Act
        activity.ChangeName("updated_name");
        
        //Assert
        Assert.NotNull(activity.UpdatedAt);
    }

    [Fact]
    public void ChangeDescription_WhenValueIsValid_UpdatesDescription()
    {
        //Arrange
        var activity = TestData.CreateActivity();
        
        //Act
        activity.ChangeDescription("updated_description");
        
        //Assert
        Assert.Equal("updated_description", activity.Description);
    }

    [Fact]
    public void ChangeDescription_WhenCalled_UpdatesUpdatedAt()
    {
        //Arrange
        var activity = TestData.CreateActivity();
        
        //Act
        activity.ChangeDescription("updated_description");
        
        //Assert
        Assert.NotNull(activity.UpdatedAt);
    }
}