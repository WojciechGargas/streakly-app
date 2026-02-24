namespace Streakly.Core.Entities;

public sealed record UserRole
{
    public static readonly UserRole Admin = new ("Admin");
    public static readonly UserRole User = new ("User");
    
    public string Value { get; }

    public UserRole(string value)
        => Value = value;
    
    public override string ToString() => Value;
    
    public static IEnumerable<UserRole> All => new[] {Admin, User};
}