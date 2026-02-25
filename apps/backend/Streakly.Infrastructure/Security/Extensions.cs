using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Streakly.Application.Security;
using Streakly.Core.Entities;

namespace Streakly.Infrastructure.Security;

internal static class Extensions
{
    public static IServiceCollection AddSecurtity(this IServiceCollection services)
    {
        services
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddSingleton<IPasswordManager, PasswordManager>();
        
        return services;
    }
}