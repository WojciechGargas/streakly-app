using Streakly.Application.Abstractions;

namespace Streakly.Application.Commands.ActivityCommands;

public record ChangeActivityDescription(Guid UserId, Guid ActivityId, string NewDescription) : ICommand;