using Microsoft.EntityFrameworkCore;
using Streakly.Application.Abstractions;
using Streakly.Application.DTO;
using Streakly.Application.Queries;

namespace Streakly.Infrastructure.DAL.Handlers;

internal sealed class GetUsersHandler(StreaklyDbContext dbContext) : IQueryHandler<GetUsers, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> HandleAsync(GetUsers query)
        => await dbContext.Users
            .AsNoTracking()
            .Select(x => x.AsDto())
            .ToListAsync();
}