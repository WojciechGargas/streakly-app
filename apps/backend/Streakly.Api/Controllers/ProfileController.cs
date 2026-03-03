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

public class ProfileController(
    ICommandHandler<ChangeUsername> changeUsernameHandler,
    IQueryHandler<GetUser, UserDto> getUserHandler)
    : ControllerBase
{
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetMe()
    {
        if (string.IsNullOrWhiteSpace(HttpContext.User.Identity?.Name))
        {
            return Unauthorized();
        }
        
        var userId = Guid.Parse(HttpContext.User.Identity.Name);
        var user = await getUserHandler.HandleAsync(new GetUser{ UserId = userId });
        if (user is null)
        {
            return NotFound();
        }
        
        return  user;
    }
    
    [Authorize]
    [HttpPatch("me/changeUsername")]
    public async Task<ActionResult> ChangeMyUsername([FromBody] ChangeMyUsername command)
    {
        if (!Guid.TryParse(User.Identity?.Name, out var userId))
        {
            return Unauthorized();
        }
        
        await changeUsernameHandler.HandleAsync(new ChangeUsername(userId, command.NewUsername));
        
        return NoContent();
    }
}