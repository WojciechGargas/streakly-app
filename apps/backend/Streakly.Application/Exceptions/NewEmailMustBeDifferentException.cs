using Streakly.Core.Exceptions;

namespace Streakly.Application.Exceptions;

public class NewEmailMustBeDifferentException : CustomException
{
    public NewEmailMustBeDifferentException() : base("New email must be different from the current email")
    {
    }
}