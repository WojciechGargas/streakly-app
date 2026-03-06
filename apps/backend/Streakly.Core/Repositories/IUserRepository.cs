using Streakly.Core.Entities;
using Streakly.Core.ValueObjects;

namespace Streakly.Core.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(UserId userId);
    Task<User?> GetUserByIdWithActivitiesAsync(UserId userId);
    Task<User?> GetUserByEmailAsync(Email email);
    Task<User?> GetUserByUsernameAsync(Username username);
    Task AddUserAsync(User user);
    Task<bool> DeleteUserByIdAsync(UserId  userId);
}