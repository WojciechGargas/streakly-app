using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streakly.Api.Exceptions;
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
    ICommandHandler<ChangeEmail>  changeEmailHandler,
    ICommandHandler<DeleteUser> deleteUserHandler,
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
    
    [HttpPatch("me/changeUsername")]
    public async Task<ActionResult> ChangeMyUsername([FromBody] ChangeMyUsername command)
    {
        var userId = GetUserId();
        
        await changeUsernameHandler.HandleAsync(new ChangeUsername(userId, command.NewUsername));
        
        return NoContent();
    }

    [HttpPatch("me/changeFullname")]
    public async Task<ActionResult> ChangeMyFullname([FromBody] ChangeMyFullname command)
    {
        var userId = GetUserId();
        
        await changeFullnameHandler.HandleAsync(new ChangeFullname(userId, command.NewFullname));
        
        return NoContent();
    }

    [HttpPatch("me/changePassword")]
    public async Task<ActionResult> ChangeMyPassword([FromBody] ChangeMyPassword command)
    {
        var userId = GetUserId();
        
        await changePasswordHandler.HandleAsync(new ChangePassword(userId, command.OldPassword, command.NewPassword));

        return NoContent();
    }

    [HttpPatch("me/changeEmail")]
    public async Task<ActionResult> ChangeMyPassword([FromBody] ChangeMyEmail command)
    {
        var userId = GetUserId();
        
        await changeEmailHandler.HandleAsync(new ChangeEmail(userId, command.NewEmail));
        
        return NoContent();
    }

    [HttpDelete("me")]
    public async Task<ActionResult> DeleteMyAccount()
    {
        var userId = GetUserId();
        
        await deleteUserHandler.HandleAsync(new DeleteUser(userId));
        
        return NoContent();
    }

    private Guid GetUserId()
    {
        if (!Guid.TryParse(User.Identity?.Name, out var userId))
        {
            throw new InvalidUserIdClaimException();
        }
        
        return userId;
    }
}