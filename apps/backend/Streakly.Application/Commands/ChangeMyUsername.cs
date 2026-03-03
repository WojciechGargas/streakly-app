using Streakly.Application.Abstractions;

namespace Streakly.Application.Commands;

public record ChangeMyUsername(string NewUsername) : ICommand;