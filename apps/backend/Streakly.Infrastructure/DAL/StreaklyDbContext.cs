using Microsoft.EntityFrameworkCore;
using Streakly.Core.Entities;

namespace Streakly.Infrastructure.DAL;

public class StreaklyDbContext(DbContextOptions<StreaklyDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}