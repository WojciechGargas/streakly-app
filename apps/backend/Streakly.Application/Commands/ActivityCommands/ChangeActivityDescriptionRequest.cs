namespace Streakly.Application.Commands.ActivityCommands;

public record ChangeActivityDescriptionRequest(Guid Id, string NewActivityDescription);