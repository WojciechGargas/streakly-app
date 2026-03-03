using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streakly.Core.Entities;
using Streakly.Core.ValueObjects;

namespace Streakly.Infrastructure.DAL.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.UserId);
        builder.Property(x => x.UserId)
            .HasConversion(x => x.Value, x => new UserId(x));
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.Email)
            .HasConversion(x => x.Value, x => new Email(x))
            .IsRequired()
            .HasMaxLength(50);
        builder.HasIndex(x => x.Username).IsUnique();
        builder.Property(x => x.Username)
            .HasConversion(x => x.Value, x => new Username(x))
            .HasMaxLength(30);
        builder.Property(x => x.Password)
            .HasConversion(x => x.Value, x => new Password(x))
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(x => x.FullName)
            .HasConversion(x => x.Value, x => new FullName(x))
            .HasMaxLength(50);
        builder.Property(x => x.Role)
            .HasConversion(x => x.Value, x => new UserRole(x))
            .HasMaxLength(20);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.LastLoggedAtUtc).IsRequired(false);
        builder.HasMany(x => x.Activities)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}