using Streakly.Application.Abstractions;

namespace Streakly.Application.Commands.ActivityCommands;

public record DeleteActivity(Guid UserId, Guid ActivityId) : ICommand;
