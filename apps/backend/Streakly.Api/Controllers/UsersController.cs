using Microsoft.AspNetCore.Mvc;
using Streakly.Application.Abstractions;
using Streakly.Application.Commands;
using Streakly.Application.DTO;
using Streakly.Application.Quaries;
using Streakly.Application.Security;

namespace Streakly.Api.Controllers;

[ApiController]
[Route("[controller]")]

public class UsersController(
    ICommandHandler<SignIn> signInHandler,
    ICommandHandler<SignUp> signUpHandler,
    IQueryHandler<GetUser, UserDto> getUserHandler,
    IQueryHandler<GetUsers, IEnumerable<UserDto>> getUsersHandler,
    ITokenStorage tokenStorage)
    : ControllerBase
{
    [HttpGet("{UserId:guid}")]
    public async Task<ActionResult<UserDto>> GetUser([FromRoute] Guid userId)
    {
        var user = await getUserHandler.HandleAsync(new GetUser{ UserId = userId });
        if(user is null)
            return NotFound();
        
        return user;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers([FromQuery] GetUsers query)
    {
        return Ok(await getUsersHandler.HandleAsync(query));
    }

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

            

