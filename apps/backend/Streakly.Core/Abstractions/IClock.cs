namespace Streakly.Core.Abstractions;

public interface IClock
{
    DateTime CurrentTimeUtc();
}