namespace Streakly.Core.Exceptions;

public class InvalidActivityDescriptionException : CustomException
{
    public string ActivityDescription { get; }

    public InvalidActivityDescriptionException(string activityDescription) 
        : base($"Activity description '{activityDescription}' is invalid.")
        => ActivityDescription = activityDescription;
}