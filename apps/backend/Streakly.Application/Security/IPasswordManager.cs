namespace Streakly.Application.Security;

public interface IPasswordManager
{
    string Secure(string password);
    bool Validate(string password, string securePassword);
}