using Streakly.Application.Abstractions;
using Streakly.Application.Exceptions;
using Streakly.Core.Repositories;
using Streakly.Core.ValueObjects;

namespace Streakly.Application.Commands.Handlers;

public class ChangeFullnameHandler(IUserRepository userRepository) : ICommandHandler<ChangeFullname>
{
    public async Task HandleAsync(ChangeFullname command)
    {
        var user = await userRepository.GetUserByIdAsync(command.UserId) ??
                   throw new UserNotFoundException(command.UserId);
        
        var fullname = new FullName(command.NewFullname);
        
        user.ChangeFullName(fullname);
    }
}