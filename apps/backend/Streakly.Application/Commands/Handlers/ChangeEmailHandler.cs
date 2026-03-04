using Streakly.Application.Abstractions;
using Streakly.Application.Exceptions;
using Streakly.Core.Repositories;
using Streakly.Core.ValueObjects;

namespace Streakly.Application.Commands.Handlers;

public class ChangeEmailHandler(IUserRepository userRepository) : ICommandHandler<ChangeEmail>
{
    public async Task HandleAsync(ChangeEmail command)
    {
        var user = await userRepository.GetUserByIdAsync(command.UserId) ??
                   throw new UserNotFoundException(command.UserId);

        if (user.Email == command.NewEmail)
        {
            throw new NewEmailMustBeDifferentException();
        }
        
        var email = new Email(command.NewEmail);

        var isTaken = await userRepository.GetUserByEmailAsync(email);
        if (isTaken is not null && isTaken.Email != command.NewEmail)
        {
            throw new EmailAlreadyInUseException(email);
        }
        
        user.ChangeEmail(command.NewEmail);
    }
}