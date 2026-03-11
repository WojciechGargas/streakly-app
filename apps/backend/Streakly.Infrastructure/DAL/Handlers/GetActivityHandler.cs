using Microsoft.EntityFrameworkCore;
using Streakly.Application.Abstractions;
using Streakly.Application.DTO;
using Streakly.Application.Exceptions;
using Streakly.Application.Queries;
using Streakly.Core.Entities;
using Streakly.Core.ValueObjects;

namespace Streakly.Infrastructure.DAL.Handlers;

internal sealed class GetActivityHandler(StreaklyDbContext dbContext) : IQueryHandler<GetActivity, ActivityDto>
{
    public async Task<ActivityDto> HandleAsync(GetActivity query)
    {
        var userId = new UserId(query.UserId);
        var activity = await dbContext.Set<Activity>()
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Id == query.ActivityId);
        
        if (activity is null)
        {
            var userExists = await dbContext.Users
                .AsNoTracking()
                .AnyAsync(x => x.UserId == userId);
            if (!userExists)
            {
                throw new UserNotFoundException(userId);
            }
            throw new ActivityNotFoundException(query.ActivityId);
        }
        
        return activity.AsDto();
    }
}