using Streakly.Application.Abstractions;

namespace Streakly.Application.Commands.ActivityCommands;

public record MarkActivityAsIncomplete(Guid UserId, Guid ActivityId) : ICommand;