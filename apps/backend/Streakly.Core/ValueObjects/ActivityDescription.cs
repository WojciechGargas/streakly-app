using Streakly.Core.Exceptions;

namespace Streakly.Core.ValueObjects;

public sealed record ActivityDescription
{
    public string Value { get; }

    public ActivityDescription(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length is < 2 or > 100)
        {
            throw new InvalidActivityDescriptionException(value);
        }
        
        Value = value;
    }
    
    public static implicit operator ActivityDescription(string value) => new(value);
    
    public static implicit operator string(ActivityDescription value) => value.Value;
    
    public override string ToString() => Value;
}