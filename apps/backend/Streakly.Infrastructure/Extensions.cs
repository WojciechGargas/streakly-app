﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Streakly.Application.Abstractions;
using Streakly.Core.Abstractions;
using Streakly.Infrastructure.Auth;
using Streakly.Infrastructure.DAL;
using Streakly.Infrastructure.Security;
using Streakly.Infrastructure.Time;

namespace Streakly.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("app");

        var infrastructureAssembly = typeof(AppOptions).Assembly;

        services.Configure<AppOptions>(section)
            .AddSecurtity()
            .AddAuth(configuration)
            .AddPostgres(configuration)
            .AddSingleton<IClock, Clock>()
            .Scan(s => s.FromAssemblies(infrastructureAssembly)
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime())
            .AddHttpContextAccessor()
            .AddSwaggerGen()
            .AddEndpointsApiExplorer();
        
        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }

    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        var section = configuration.GetSection(sectionName);
        section.Bind(options);
        
        return options;
    }
}