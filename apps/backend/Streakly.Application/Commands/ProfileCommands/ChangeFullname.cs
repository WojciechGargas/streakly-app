using Streakly.Application.Abstractions;

namespace Streakly.Application.Commands;

public record ChangeFullname(Guid UserId, string NewFullname) : ICommand;