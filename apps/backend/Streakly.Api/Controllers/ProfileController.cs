using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streakly.Application.Abstractions;
using Streakly.Application.Commands;
using Streakly.Application.DTO;
using Streakly.Application.Quaries;
using Streakly.Application.Security;

namespace Streakly.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]

public class ProfileController(
    ICommandHandler<ChangeUsername> changeUsernameHandler,
    ICommandHandler<ChangeFullname> changeFullnameHandler,
    ICommandHandler<ChangePassword> changePasswordHandler,
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
        
        var userId = GetUserId();
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
        var userId = GetUserId();
        
        await changeUsernameHandler.HandleAsync(new ChangeUsername(userId, command.NewUsername));
        
        return NoContent();
    }
    
    [Authorize]
    [HttpPatch("me/changeFullname")]
    public async Task<ActionResult> ChangeMyFullname([FromBody] ChangeMyFullname command)
    {
        var userId = GetUserId();
        
        await changeFullnameHandler.HandleAsync(new ChangeFullname(userId, command.NewFullname));
        
        return NoContent();
    }

    [Authorize]
    [HttpPatch("me/changePassword")]
    public async Task<ActionResult> ChangeMyPassword([FromBody] ChangeMyPassword command)
    {
        var userId = GetUserId();
        
        await changePasswordHandler.HandleAsync(new ChangePassword(userId, command.OldPassword, command.NewPassword));

        return NoContent();
    }

    private Guid GetUserId()
    {
        if (!Guid.TryParse(User.Identity?.Name, out var userId))
        {
            throw new UnauthorizedAccessException();
        }
        
        return userId;
    }
}