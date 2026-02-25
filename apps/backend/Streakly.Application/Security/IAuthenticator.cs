using Streakly.Application.DTO;

namespace Streakly.Application.Security;

public interface IAuthenticator
{
    JwtDto CreateToken(Guid userId, string role);
}