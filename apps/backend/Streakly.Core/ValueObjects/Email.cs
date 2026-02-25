using System.Text.RegularExpressions;
using Streakly.Core.Exceptions;

namespace Streakly.Core.ValueObjects;

public sealed record Email
{
    private static readonly Regex Regex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]{2,}$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public string Value { get; }

    public Email(string value)
    {
        value = value.ToLowerInvariant();
        
        if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value) || value.Length > 60)
        {
            throw new InvalidEmailException(value);
        }
        
        Value = value;
    }
}