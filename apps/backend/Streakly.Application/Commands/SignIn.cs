using Streakly.Application.Abstractions;

namespace Streakly.Application.Commands;

public record SignIn(string Email, string Password) : ICommand;
