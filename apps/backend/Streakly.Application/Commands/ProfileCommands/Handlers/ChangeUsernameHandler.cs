using Streakly.Application.Abstractions;
using Streakly.Application.Exceptions;
using Streakly.Core.Repositories;
using Streakly.Core.ValueObjects;

namespace Streakly.Application.Commands.Handlers;

internal sealed class ChangeUsernameHandler(IUserRepository userRepository) : ICommandHandler<ChangeUsername>
{
    public async Task HandleAsync(ChangeUsername command)
    {
        var user = await userRepository.GetUserByIdAsync(command.UserId) ??
                   throw new UserNotFoundException(command.UserId);
        
        var username = new Username(command.NewUsername);

        var isTaken = await userRepository.GetUserByUsernameAsync(username);
        if (isTaken is not null && isTaken.UserId != user.UserId)
        {
            throw new UsernameAlreadyInUseException(username);
        }
        
        user.ChangeUsername(command.NewUsername);
    }
}