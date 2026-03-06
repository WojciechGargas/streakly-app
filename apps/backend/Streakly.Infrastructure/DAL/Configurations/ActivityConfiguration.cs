using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streakly.Core.Entities;
using Streakly.Core.ValueObjects;

namespace Streakly.Infrastructure.DAL.Configurations;

internal sealed class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        builder.Property(x => x.UserId)
            .HasConversion(x => x.Value, x => new UserId(x));
        builder.Property(x => x.Name)
            .HasConversion(x => x.Value, x => new ActivityName(x));
        builder.Property(x => x.Description)
            .HasConversion(x => x.Value, x => new ActivityDescription(x));
    }
}
