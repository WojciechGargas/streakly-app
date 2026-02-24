using Streakly.Core.ValueObjects;

namespace Streakly.Core.Entities;

public class User
{

    public UserId UserId { get; private set; }
    public Email Email { get; private set; }
    public Username Username { get; private set; }
    public Password Password { get; private set; }
    public FullName FullName { get; private set; }
    public UserRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    
    public User(UserId userId, Email email, Username username, Password password,
        FullName fullName, UserRole role, DateTime createdAt)
    {
        UserId = userId;
        Email = email;
        Username = username;
        Password = password;
        FullName = fullName;
        Role = role;
        CreatedAt = createdAt;
    }
}
