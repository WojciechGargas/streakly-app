namespace Streakly.Application.Commands.ActivityCommands;

public record ChangeActivityNameRequest(Guid Id, string NewActivityName);