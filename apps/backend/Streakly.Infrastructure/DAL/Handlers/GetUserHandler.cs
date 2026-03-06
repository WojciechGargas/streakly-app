using Microsoft.EntityFrameworkCore;
using Streakly.Application.Abstractions;
using Streakly.Application.DTO;
using Streakly.Application.Exceptions;
using Streakly.Application.Queries;
using Streakly.Core.ValueObjects;

namespace Streakly.Infrastructure.DAL.Handlers;

internal sealed class GetUserHandler(StreaklyDbContext dbContext) : IQueryHandler<GetUser, UserDto>
{
    public async Task<UserDto> HandleAsync(GetUser query)
    {
        var userId = new UserId(query.UserId);
        var user = await dbContext.Users
            .Include(a => a.Activities)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == userId);

        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }

        return user.AsDto();
    }
}