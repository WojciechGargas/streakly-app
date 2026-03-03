using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Streakly.Application.Abstractions;
using Streakly.Core.Repositories;
using Streakly.Infrastructure.DAL.Decorators;
using Streakly.Infrastructure.DAL.Repositories;

namespace Streakly.Infrastructure.DAL;

internal static class Extensions
{
    private const string SectionName = "postgres";

    public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(SectionName);
        services.Configure<PostgresOptions>(section);
        var options = configuration.GetOptions<PostgresOptions>(SectionName);

        services.AddDbContext<StreaklyDbContext>(x => x.UseNpgsql(options.ConnectionString));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, PostgresUnitOfWork>();
        services.TryDecorate(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));

        return services;
    }
}