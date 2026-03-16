using Streakly.Core.Entities;
using Streakly.Core.ValueObjects;

namespace Streakly.Tests.Unit.Shared;

public static class TestData
{
    public static User CreateUser(
        Guid? userId = null,
        string email = "test@streakly.test",
        string username = "testuser",
        string password = "User123!",
        string fullName = "Test User")
        => new(
            new UserId(userId ?? Guid.NewGuid()),
            new Email(email),
            new Username(username),
            new Password(password),
            new FullName(fullName),
            UserRole.User,
            DateTime.UtcNow,
            new List<Activity>());

    public static Activity CreateActivity(
        UserId? userId = null,
        string name = "Run 5 km",
        string description = "Morning running")
    {
        var now = DateTime.UtcNow;
        return Activity.Create(
            userId ?? new UserId(Guid.NewGuid()),
            new ActivityName(name),
            new ActivityDescription(description),
            now,
            now,
            null,
            ActivityFrequencyType.Daily);
    }
}