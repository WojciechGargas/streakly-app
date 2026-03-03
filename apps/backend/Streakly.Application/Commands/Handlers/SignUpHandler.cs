using Streakly.Application.Abstractions;
using Streakly.Application.Exceptions;
using Streakly.Application.Security;
using Streakly.Core.Abstractions;
using Streakly.Core.Entities;
using Streakly.Core.Repositories;
using Streakly.Core.ValueObjects;

namespace Streakly.Application.Commands.Handlers;

public class SignUpHandler(IUserRepository userRepository, IPasswordManager passwordManager, IClock clock)
    : ICommandHandler<SignUp>
{


    public async Task HandleAsync(SignUp command)
    {
        var userId = new UserId(command.UserId);
        var email = new Email(command.Email);
        var username = new Username(command.Username);
        var password = new Password(command.Password);
        var fullName = new FullName(command.FullName);
        var role = string.IsNullOrWhiteSpace(command.Role) ? UserRole.User : new UserRole(command.Role);

        if (await userRepository.GetUserByEmailAsync(email) is not null)
        {
            throw new EmailAlreadyInUseException(email);
        }

        if (await userRepository.GetUserByUsernameAsync(username) is not null)
        {
            throw new UsernameAlreadyInUseException(username);
        }

        var securedPassword = passwordManager.Secure(password);
        var user = new User(userId, email, username, securedPassword, fullName,
            role, clock.CurrentTimeUtc(), new List<Activity>());
        
        await userRepository.AddUserAsync(user);
    }
}