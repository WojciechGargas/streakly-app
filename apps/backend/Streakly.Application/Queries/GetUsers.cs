using Streakly.Application.Abstractions;
using Streakly.Application.DTO;

namespace Streakly.Application.Queries;

public class GetUsers : IQuery<IEnumerable<UserDto>>
{
}