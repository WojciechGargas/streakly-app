using Microsoft.Extensions.Internal;
using Streakly.Core.Abstractions;

namespace Streakly.Tests.Integration.Infrastructure;

public sealed class TestClock : IClock
{
    public DateTime CurrentTime { get; set; } = new (2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    public DateTime CurrentTimeUtc() => CurrentTime;
}