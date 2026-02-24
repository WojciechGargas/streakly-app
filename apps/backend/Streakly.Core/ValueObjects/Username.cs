using Streakly.Core.Exceptions;

namespace Streakly.Core.ValueObjects;

public sealed record Username
{
    public string Value { get; }

    public Username(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length is < 3 or > 20)
        {
            throw new InvalidUsernameException(value);
        }
        
        Value = value;
    }
    
    public static implicit operator Username(string value) =>  new Username(value);
    
    public static implicit operator string(Username value) => value.Value;
    
    public override string ToString() => Value;
}