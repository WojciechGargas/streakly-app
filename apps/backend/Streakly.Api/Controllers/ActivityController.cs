using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streakly.Api.Exceptions;
using Streakly.Application.Abstractions;
using Streakly.Application.Commands.ActivityCommands;

namespace Streakly.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]

public class ActivityController(
    ICommandHandler<AddActivity> addActivityCommandHandler)
    : ControllerBase
{
    [HttpPost("addActivity/")]
    public async Task<ActionResult> AddActivity(AddActivityRequest request)
    {
        var userId = GetUserId();
        var command = new AddActivity(
            userId,
            request.Name,
            request.Description,
            request.StartDate,
            request.EndDate,
            request.FrequencyType);
        
        await addActivityCommandHandler.HandleAsync(command);
        
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