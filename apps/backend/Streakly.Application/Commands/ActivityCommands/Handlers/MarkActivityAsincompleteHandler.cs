using Streakly.Application.Abstractions;
using Streakly.Application.Exceptions;
using Streakly.Core.Repositories;

namespace Streakly.Application.Commands.ActivityCommands.Handlers;

public class MarkActivityAsincompleteHandler(IUserRepository userRepository) : ICommandHandler<MarkActivityAsIncomplete>
{
    public async Task HandleAsync(MarkActivityAsIncomplete command)
    {
        var user = await userRepository.GetUserByIdWithActivitiesAsync(command.UserId) ??
                   throw new UserNotFoundException(command.UserId);
        
        user.MarkActivityAsIncompleted(command.ActivityId);
    }
}