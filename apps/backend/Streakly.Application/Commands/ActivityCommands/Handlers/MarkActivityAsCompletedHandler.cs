using Streakly.Application.Abstractions;
using Streakly.Application.Exceptions;
using Streakly.Core.Repositories;

namespace Streakly.Application.Commands.ActivityCommands.Handlers;

public class MarkActivityAsCompletedHandler(IUserRepository userRepository) : ICommandHandler<MarkActivityAsCompleted>
{
    public async Task HandleAsync(MarkActivityAsCompleted command)
    {
        var user = await userRepository.GetUserByIdWithActivitiesAsync(command.UserId) ??
                   throw new UserNotFoundException(command.UserId);
        
        user.MarkActivityAsCompleted(command.ActivityId);
    }
}