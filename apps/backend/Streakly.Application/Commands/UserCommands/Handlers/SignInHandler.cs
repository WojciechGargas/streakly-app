using Streakly.Application.Abstractions;
using Streakly.Application.Exceptions;
using Streakly.Application.Security;
using Streakly.Core.Abstractions;
using Streakly.Core.Repositories;

namespace Streakly.Application.Commands.Handlers;

internal sealed class SignInHandler(
    IUserRepository userRepository,
    IAuthenticator authenticator,
    IPasswordManager passwordManager,
    ITokenStorage tokenStorage,
    IClock clock
    ) : ICommandHandler<SignIn>
{
    public async Task HandleAsync(SignIn command)
    {
        var user = await userRepository.GetUserByEmailAsync(command.Email) ??
                   throw new InvalidCredentialsException();

        if (!passwordManager.Validate(command.Password, user.Password))
        {
            throw new InvalidCredentialsException();
        }
        
        user.MarkAsLoggedIn(clock.CurrentTimeUtc());
        
        var jwt = authenticator.CreateToken(user.UserId, user.Role.ToString());
        
        tokenStorage.Set(jwt);
    }
}