using Microsoft.AspNetCore.Http;
using Streakly.Application.DTO;
using Streakly.Application.Security;

namespace Streakly.Infrastructure.Auth;

internal sealed class HttpContextTokenStorage(IHttpContextAccessor httpContextAccessor) : ITokenStorage
{
    private const string TokenKey = "jwt";
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public void Set(JwtDto jwt)
        => httpContextAccessor.HttpContext?.Items.TryAdd(TokenKey, jwt);

    public JwtDto Get()
    {
        if (httpContextAccessor.HttpContext is null)
            return null;

        if (httpContextAccessor.HttpContext.Items.TryGetValue(TokenKey, out var jwt))
        {
            return jwt as JwtDto;
        }
        
        return null;
    }
}