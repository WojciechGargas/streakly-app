using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Streakly.Application.Security;
using Streakly.Core.Abstractions;
using Streakly.Core.Entities;
using Streakly.Infrastructure.DAL;
using Testcontainers.PostgreSql;

namespace Streakly.Tests.Integration.Infrastructure;

public sealed class ApplicationWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:17")
        .WithDatabase("streakly_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private string? _connectionString;

    public TestClock Clock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IClock>();
            services.AddSingleton<IClock>(Clock);
        });

        builder.ConfigureTestServices(testServices =>
        {
            var descriptor =
                testServices.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<StreaklyDbContext>));

            if (descriptor is not null)
            {
                testServices.Remove(descriptor);
            }

            testServices.AddDbContext<StreaklyDbContext>(options =>
            {
                options.UseNpgsql(_connectionString ??
                                  throw new InvalidOperationException("Test database was not initialized"));
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        _connectionString = _dbContainer.GetConnectionString();

        await ResetDatabaseAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
    }

    private async Task ResetDatabaseAsync()
    {
        var options = new DbContextOptionsBuilder<StreaklyDbContext>()
            .UseNpgsql(_connectionString ?? throw new InvalidOperationException("Test database was not initialized"))
            .Options;

        await using var dbContext = new StreaklyDbContext(options);
        await dbContext.Database.MigrateAsync();

        await dbContext.Users.ExecuteDeleteAsync();

        await using var scope = Services.CreateAsyncScope();
        var passwordManager = scope.ServiceProvider.GetRequiredService<IPasswordManager>();

        var users = new List<User>
        {
            new(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "admin@streakly.test",
                "admin",
                passwordManager.Secure("Admin123!"),
                "Admin User",
                UserRole.Admin,
                DateTime.UtcNow.AddDays(-30),
                new List<Activity>()),

            new(
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                "anna@streakly.test",
                "anna",
                passwordManager.Secure("User123!"),
                "Anna Kowalska",
                UserRole.User,
                DateTime.UtcNow.AddDays(-20),
                new List<Activity>()),

            new(
                Guid.Parse("33333333-3333-3333-3333-333333333333"),
                "jan@streakly.test",
                "janek",
                passwordManager.Secure("User123!"),
                "Jan Nowak",
                UserRole.User,
                DateTime.UtcNow.AddDays(-10),
                new List<Activity>()),

            new(
                Guid.Parse("44444444-4444-4444-4444-444444444444"),
                "ola@streakly.test",
                "ola123",
                passwordManager.Secure("User123!"),
                "Ola Wisniewska",
                UserRole.User,
                DateTime.UtcNow.AddDays(-5),
                new List<Activity>()),

            new(
                Guid.Parse("55555555-5555-5555-5555-555555555555"),
                "michal@streakly.test",
                "michal",
                passwordManager.Secure("User123!"),
                "Michal Zielinski",
                UserRole.User,
                DateTime.UtcNow.AddDays(-2),
                new List<Activity>())
        };

        await dbContext.Users.AddRangeAsync(users);
        await dbContext.SaveChangesAsync();
    }
}
