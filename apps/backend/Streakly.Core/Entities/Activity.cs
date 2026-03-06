using Streakly.Core.ValueObjects;

namespace Streakly.Core.Entities;

public enum ActivityFrequencyType
{
    Single,
    Daily,
    Weekly,
    Monthly,
    Yearly
}

public class Activity
{
    public Guid Id { get; private set; }
    public UserId UserId { get; private set; }
    public ActivityName Name { get; private set; }
    public ActivityDescription Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public ActivityFrequencyType Type{ get; private set; }
    public bool Completed { get; private set; } 
    
    public Activity(Guid id, UserId userId, ActivityName name, ActivityDescription description,
        DateTime createdAt, DateTime? updatedAt, DateTime startDate, DateTime? endDate,
        ActivityFrequencyType type, bool completed)
    {
        Id = id;
        UserId = userId;
        Name = name;
        Description = description;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        StartDate = startDate;
        EndDate = endDate;
        Type = type;
        Completed = completed;
    }

    public Activity()
    {
    }

    public static Activity Create(
        UserId userId,
        ActivityName name,
        ActivityDescription description,
        DateTime createdAt,
        DateTime startDate,
        DateTime? endDate,
        ActivityFrequencyType type)
    {
        return new Activity(
            Guid.NewGuid(),
            userId,
            name,
            description,
            createdAt,
            null,
            startDate,
            endDate,
            type,
            false);
    }
}