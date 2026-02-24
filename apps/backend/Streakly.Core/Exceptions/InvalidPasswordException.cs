namespace Streakly.Core.Exceptions;

public class InvalidPasswordException : CustomException
{
    public string Password { get; }

    public InvalidPasswordException(string password) : base($"Provided password '{password}' is not a valid password.")
        => Password = password;

}