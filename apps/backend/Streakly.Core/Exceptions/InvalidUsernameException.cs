namespace Streakly.Core.Exceptions;

public class InvalidUsernameException : CustomException
{
    public string UserName { get; }

    public InvalidUsernameException(string userName) : base($"Username {userName} is not a valid username.")
        => UserName = userName;
}