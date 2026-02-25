using Streakly.Core.Abstractions;

namespace Streakly.Infrastructure.Time;

public class Clock :  IClock
{
    DateTime IClock.CurrentTimeUtc()
        => DateTime.UtcNow;
}