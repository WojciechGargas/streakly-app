using Streakly.Application.Abstractions;
using Streakly.Application.DTO;

namespace Streakly.Application.Queries;

public class GetUser : IQuery<UserDto>
{
    public Guid UserId { get; set; }
}