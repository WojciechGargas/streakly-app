using Microsoft.EntityFrameworkCore;
using Streakly.Core.Entities;
using Streakly.Core.Repositories;
using Streakly.Core.ValueObjects;

namespace Streakly.Infrastructure.DAL.Repositories;

public class UserRepository(StreaklyDbContext dbContext) : IUserRepository
{
    private readonly DbSet<User> _users = dbContext.Users;
    
    public Task<User?> GetUserByIdAsync(UserId id)
        => _users.SingleOrDefaultAsync(x => x.UserId == id);

    public Task<User?> GetUserByEmailAsync(Email email)
        => _users.SingleOrDefaultAsync(x => x.Email == email);

    public Task<User?> GetUserByUsernameAsync(Username username)
        => _users.SingleOrDefaultAsync(x => x.Username == username);

    public async Task AddUserAsync(User user)
        => await _users.AddAsync(user);
}