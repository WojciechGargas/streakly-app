using Streakly.Application.Abstractions;

namespace Streakly.Application.Commands;

public record ChangeUsername(Guid UserId, string NewUsername) : ICommand;
