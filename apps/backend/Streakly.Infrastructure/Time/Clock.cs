using Streakly.Core.Abstractions;

namespace Streakly.Infrastructure.Time;

public class Clock :  IClock
{
    public DateTime CurrentTimeUtc => DateTime.UtcNow;
}