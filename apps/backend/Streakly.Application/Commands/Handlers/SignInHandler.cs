using System.Security.Authentication;
using Streakly.Application.Abstractions;
using Streakly.Application.Exceptions;
using Streakly.Application.Security;
using Streakly.Core.Repositories;

namespace Streakly.Application.Commands.Handlers;

internal sealed class SignInHandler(
    IUserRepository userRepository,
    IAuthenticator authenticator,
    IPasswordManager passwordManager,
    ITokenStorage tokenStorage
    ) : ICommandHandler<SignIn>
{
    public async Task HandleAsync(SignIn command)
    {
        var user = await userRepository.GetUserByEmailAsync(command.Email);
        if (user is null)
        {
            throw new InvalidCredentialsException();
        }

        if (!passwordManager.Validate(command.Password, user.Password))
        {
            throw new InvalidCredentialsException();
        }

        var jwt = authenticator.CreateToken(user.UserId, user.Role.ToString());
        
        tokenStorage.Set(jwt);
    }
}