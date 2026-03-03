using Microsoft.AspNetCore.Mvc;
using Streakly.Application.Abstractions;
using Streakly.Application.Commands;
using Streakly.Application.Security;

namespace Streakly.Api.Controllers;

[ApiController]
[Route("[controller]")]

public class AuthController(
    ICommandHandler<SignIn> signInHandler,
    ICommandHandler<SignUp> signUpHandler,
    ITokenStorage tokenStorage)
    : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> SignUp(SignUp command)
    {
        command = command with { UserId = Guid.NewGuid() };
        await signUpHandler.HandleAsync(command);

        return Created();
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult> SignIn(SignIn command)
    {
        await signInHandler.HandleAsync(command);
        var jwt = tokenStorage.Get();
        
        return Ok(jwt);
    }
}