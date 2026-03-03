using Streakly.Application.Abstractions;
using Streakly.Application.Exceptions;
using Streakly.Core.Repositories;

namespace Streakly.Application.Commands.Handlers;

internal sealed class DeleteUserHandler(IUserRepository userRepository) : ICommandHandler<DeleteUser> 
{
    public async Task HandleAsync(DeleteUser command)
    {
        var deleted = await userRepository.DeleteUserByIdAsync(command.UserId);
        if (!deleted)
        {
            throw new UserNotFoundException(command.UserId);
        }
    }
}