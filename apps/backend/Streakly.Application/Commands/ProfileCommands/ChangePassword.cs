using Streakly.Application.Abstractions;

namespace Streakly.Application.Commands;

public record ChangePassword(Guid UserId, string OldPassword, string NewPassword) : ICommand;