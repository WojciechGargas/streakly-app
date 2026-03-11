using Streakly.Application.Abstractions;
using Streakly.Application.DTO;

namespace Streakly.Application.Queries;

public class GetActivity : IQuery<ActivityDto>
{
    public Guid UserId { get; set; }
    public Guid ActivityId { get; set; }
}