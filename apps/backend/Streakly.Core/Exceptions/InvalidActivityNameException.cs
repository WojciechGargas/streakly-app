namespace Streakly.Core.Exceptions;

public class InvalidActivityNameException : CustomException
{
    public string ActivityName { get; }

    public InvalidActivityNameException(string activityName) : base($"Activity name '{activityName}' is invalid.")
        => ActivityName = activityName;

}