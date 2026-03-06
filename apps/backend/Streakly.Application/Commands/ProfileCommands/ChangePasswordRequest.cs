namespace Streakly.Application.Commands;

public record ChangePasswordRequest(string OldPassword, string NewPassword);
