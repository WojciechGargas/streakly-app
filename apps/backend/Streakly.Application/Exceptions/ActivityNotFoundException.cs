using Streakly.Core.Exceptions;

namespace Streakly.Application.Exceptions;

public class ActivityNotFoundException : CustomException
{
    public  Guid ActivityId;

    public ActivityNotFoundException(Guid activityId) : base($"Activity with ID : '{activityId}' was not found.")
    {
        ActivityId = activityId;
    }
}