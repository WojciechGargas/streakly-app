using Streakly.Application.Abstractions;

namespace Streakly.Application.Commands;

public record DeleteUser(Guid UserId) : ICommand;
