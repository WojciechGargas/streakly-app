using Microsoft.AspNetCore.Identity;
using Streakly.Application.Security;
using Streakly.Core.Entities;

namespace Streakly.Infrastructure.Security;

public class PasswordManager(IPasswordHasher<User> passwordHasher) : IPasswordManager
{
    public string Secure(string password)
        => passwordHasher.HashPassword(null!,  password);

    public bool Validate(string password, string securePassword)
        => passwordHasher.VerifyHashedPassword(null!, securePassword, password)
            is PasswordVerificationResult.Success;
}