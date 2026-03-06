using Streakly.Core.Entities;
using Streakly.Core.ValueObjects;

namespace Streakly.Application.Commands.ActivityCommands;

public record AddActivityRequest(
    ActivityName Name,
    ActivityDescription Description,
    DateTime StartDate,
    DateTime EndDate,
    ActivityFrequencyType FrequencyType);