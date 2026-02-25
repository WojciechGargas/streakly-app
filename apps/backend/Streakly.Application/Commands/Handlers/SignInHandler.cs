using System.Security.Authentication;
using Streakly.Application.Abstractions;
using Streakly.Application.Exceptions;
using Streakly.Core.Repositories;

namespace Streakly.Application.Commands.Handlers;

internal sealed class SignInHandler(IUserRepository userRepository) : ICommandHandler<SignIn>
{
    public async Task HandleAsync(SignIn command)
    {
        var user = await userRepository.GetUserByEmailAsync(command.Email);
        if (user is null)
        {
            throw new InvalidCredentialsException();
        }
    }
}