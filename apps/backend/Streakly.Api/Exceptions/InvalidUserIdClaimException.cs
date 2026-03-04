using Streakly.Core.Exceptions;

namespace Streakly.Api.Exceptions;

public class InvalidUserIdClaimException : CustomException
{
    public InvalidUserIdClaimException() : base("User ID claim is missing or invalid")
    {
    }
}