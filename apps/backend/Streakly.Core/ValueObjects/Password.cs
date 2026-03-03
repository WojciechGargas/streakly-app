using Streakly.Core.Exceptions;

namespace Streakly.Core.ValueObjects;

public sealed record Password
{
    public string Value { get; }

    public Password(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length is < 6 or > 100)
        {
            throw new InvalidPasswordException(value);
        }
        
        Value = value;
    }
    
    public static implicit operator Password(string value) => new Password(value);
    
    public static implicit operator string(Password password) => password.Value;
    
    public override string ToString() => Value;
}