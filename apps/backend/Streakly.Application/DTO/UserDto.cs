using Streakly.Core.Entities;

namespace Streakly.Application.DTO;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime? LastLoggedIn { get; set; }
    public List<Activity> Activities { get; set; } = new();
}