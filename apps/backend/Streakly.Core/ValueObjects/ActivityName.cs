using Streakly.Core.Exceptions;

namespace Streakly.Core.ValueObjects;

public sealed record ActivityName
{
    public string Value { get; }

    public ActivityName(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length is < 2 or > 50)
        {
            throw new InvalidActivityNameException(value);
        }
        
        Value = value;
    }
    
    public static implicit operator ActivityName(string value) => new(value);
    
    public static implicit operator string(ActivityName value) => value.Value;
    
    public override string ToString() => Value;
}