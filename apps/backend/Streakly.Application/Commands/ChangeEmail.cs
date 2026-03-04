using Streakly.Application.Abstractions;

namespace Streakly.Application.Commands;

public record ChangeEmail(Guid UserId, string NewEmail) : ICommand;