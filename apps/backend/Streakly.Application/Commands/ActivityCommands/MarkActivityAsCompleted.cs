using Streakly.Application.Abstractions;

namespace Streakly.Application.Commands.ActivityCommands;

public record MarkActivityAsCompleted(Guid UserId, Guid ActivityId) : ICommand;
