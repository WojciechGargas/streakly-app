using Streakly.Application.Abstractions;
using Streakly.Application.Exceptions;
using Streakly.Core.Repositories;

namespace Streakly.Application.Commands.ActivityCommands.Handlers;

public class ChangeActivityNameHandler(IUserRepository userRepository) : ICommandHandler<ChangeActivityName>
{
    public async Task HandleAsync(ChangeActivityName command)
    {
        var user = await userRepository.GetUserByIdWithActivitiesAsync(command.UserId) ??
                   throw new UserNotFoundException(command.UserId);
        
        user.ChangeActivityName(command.ActivityId, command.NewName);
    }
}