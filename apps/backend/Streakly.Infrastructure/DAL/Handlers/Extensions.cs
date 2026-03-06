using Microsoft.EntityFrameworkCore.ChangeTracking;
using Streakly.Application.DTO;
using Streakly.Core.Entities;

namespace Streakly.Infrastructure.DAL.Handlers;

internal static class Extensions
{
    public static UserDto AsDto(this User entity)
        => new()
        {
            Id = entity.UserId,
            Username = entity.Username,
            FullName = entity.FullName,
            LastLoggedIn = entity.LastLoggedAtUtc,
            Activities = entity.Activities
        };
}