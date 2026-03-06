namespace Streakly.Core.Exceptions;

public class ActivityNotFoundException : CustomException
{
    public Guid ActivityId { get; }

    public ActivityNotFoundException(Guid activityId)
        : base($"Activity with ID : '{activityId}' was not found.")
    {
        ActivityId = activityId;
    }
}
