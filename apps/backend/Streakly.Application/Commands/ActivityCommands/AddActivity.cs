using Streakly.Application.Abstractions;
using Streakly.Core.Entities;
using Streakly.Core.ValueObjects;

namespace Streakly.Application.Commands.ActivityCommands;

public record AddActivity(
    Guid Id,
    UserId UserId,
    ActivityName Name,
    ActivityDescription Description,
    DateTime StartDate,
    DateTime EndDate,
    ActivityFrequencyType FrequencyType) 
    : ICommand;
