using Streakly.Core.Entities;

namespace Streakly.Application.DTO;

public class ActivityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ActivityFrequencyType Type { get; set; }
    public bool Completed { get; set; } 
}