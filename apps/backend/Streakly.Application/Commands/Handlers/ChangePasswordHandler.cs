using System.Security.Authentication;
using Streakly.Application.Abstractions;
using Streakly.Application.Exceptions;
using Streakly.Application.Security;
using Streakly.Core.Repositories;
using Streakly.Core.ValueObjects;

namespace Streakly.Application.Commands.Handlers;

public class ChangePasswordHandler(
    IUserRepository userRepository,
    IPasswordManager passwordManager)
    : ICommandHandler<ChangePassword>
{
    public async Task HandleAsync(ChangePassword command)
    {
        var user = await userRepository.GetUserByIdAsync(command.UserId) ??
                   throw new UserNotFoundException(command.UserId);

        if (!passwordManager.Validate(command.OldPassword, user.Password))
        {
            throw new InvalidCredentialsException();
        }
        
        var newPassword = new Password(command.NewPassword);
        var securedPassword = passwordManager.Secure(newPassword);
        user.ChangePassword(securedPassword);
    }
}