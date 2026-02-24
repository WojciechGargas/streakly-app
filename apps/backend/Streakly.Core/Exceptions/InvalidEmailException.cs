namespace Streakly.Core.Exceptions;

public class InvalidEmailException : CustomException
{
    public string Email { get; }
    
    public InvalidEmailException(string email) : base($"Email: {email} is not a valid e-mail address.")
        => Email = email;
}