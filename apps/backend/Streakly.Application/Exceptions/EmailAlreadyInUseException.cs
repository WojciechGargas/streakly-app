using Streakly.Core.Exceptions;

namespace Streakly.Application.Exceptions;

public class EmailAlreadyInUseException : CustomException
{
    public string Email { get; }
    
    public EmailAlreadyInUseException(string email) : base($"Email '{email}' is already in use")
    {
        Email = email;
    }
}