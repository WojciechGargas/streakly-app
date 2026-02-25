using Streakly.Core.Exceptions;

namespace Streakly.Application.Exceptions;

public class InvalidCredentialsException : CustomException
{
    public InvalidCredentialsException() : base("Invalid credentials")
    {
    }
}