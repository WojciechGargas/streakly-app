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
    public DateTime UpdatedAt { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public ActivityFrequencyType Type{ get; private set; }
    public bool Completed { get; private set; }
    
}