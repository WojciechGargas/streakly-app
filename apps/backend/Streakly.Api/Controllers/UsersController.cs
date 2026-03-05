using Microsoft.AspNetCore.Authorization;
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
    ICommandHandler<DeleteUser> deleteUserHandler,
    IQueryHandler<GetUser, UserDto> getUserHandler,
    IQueryHandler<GetUsers, IEnumerable<UserDto>> getUsersHandler)
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
    
    [HttpDelete("{UserId:guid}")]
    [Authorize(Policy = "is-admin")]
    public async Task<ActionResult> DeleteUser([FromRoute] Guid userId)
    {
        await deleteUserHandler.HandleAsync(new DeleteUser(userId));
        
        return NoContent();
    }
}

            

