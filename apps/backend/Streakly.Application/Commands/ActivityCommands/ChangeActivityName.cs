using Streakly.Application.Abstractions;

namespace Streakly.Application.Commands.ActivityCommands;

public record ChangeActivityName(Guid UserId, Guid ActivityId, string NewName) : ICommand;