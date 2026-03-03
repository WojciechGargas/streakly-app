using Streakly.Application.Abstractions;

namespace Streakly.Application.Commands;

public record ChangeMyFullname(string NewFullname) : ICommand;