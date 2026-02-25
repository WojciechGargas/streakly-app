using Streakly.Application.Abstractions;
using Streakly.Application.DTO;

namespace Streakly.Application.Quaries;

public class GetUsers : IQuery<IEnumerable<UserDto>>
{
}