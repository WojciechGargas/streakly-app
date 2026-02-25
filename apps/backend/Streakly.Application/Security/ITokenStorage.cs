using Streakly.Application.DTO;

namespace Streakly.Application.Security;

public interface ITokenStorage
{
    void Set(JwtDto jwt);
    JwtDto Get();
}