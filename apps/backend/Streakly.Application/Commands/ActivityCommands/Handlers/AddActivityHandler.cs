using Streakly.Application.Abstractions;
using Streakly.Application.Exceptions;
using Streakly.Core.Entities;
using Streakly.Core.Repositories;

namespace Streakly.Application.Commands.ActivityCommands.Handlers;

public class AddActivityHandler(IUserRepository userRepository) : ICommandHandler<AddActivity>
{
    public async Task HandleAsync(AddActivity command)
    {
        var user = await userRepository.GetUserByIdAsync(command.UserId) ??
                   throw new UserNotFoundException(command.UserId);

        var startDate = EnsureUtc(command.StartDate);
        var endDate = EnsureUtc(command.EndDate);

        var activityToAdd = Activity.Create(
            user.UserId,
            command.Name,
            command.Description,
            DateTime.UtcNow,
            startDate,
            endDate,
            command.FrequencyType
        );
        
        user.AddActivity(activityToAdd);
    }

    private static DateTime EnsureUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
        _ => value
    };
}
