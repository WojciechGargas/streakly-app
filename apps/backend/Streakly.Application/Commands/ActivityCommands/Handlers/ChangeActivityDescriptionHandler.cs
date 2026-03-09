using Streakly.Application.Abstractions;
using Streakly.Application.Exceptions;
using Streakly.Core.Repositories;

namespace Streakly.Application.Commands.ActivityCommands.Handlers;

public class ChangeActivityDescriptionHandler(IUserRepository userRepository) : ICommandHandler<ChangeActivityDescription>
{
    public async Task HandleAsync(ChangeActivityDescription command)
    {
        var user = await userRepository.GetUserByIdWithActivitiesAsync(command.UserId) ??
                   throw new UserNotFoundException(command.UserId);
        
        user.ChangeActivityDescription(command.ActivityId, command.NewDescription);
    }
}