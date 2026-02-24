namespace Streakly.Core.Exceptions;

public class InvalidFullNameException : CustomException
{
    public string FullName { get; }

    public InvalidFullNameException(string fullName) : base($"Full name '{fullName}' is not a valid full name.")
        => FullName = fullName;

}