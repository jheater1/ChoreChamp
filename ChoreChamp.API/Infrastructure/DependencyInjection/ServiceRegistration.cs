﻿using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Infrastructure.Security;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Infrastructure.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextServices(configuration);
        services.AddFastEndpointsServices();
        services.AddValidationServices();
        services.AddAuthenticationServices(configuration);
        services.AddAuthorizationServices(configuration);
        services.AddPasswordService(configuration);
        return services;
    }

    private static IServiceCollection AddValidationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        return services;
    }

    private static IServiceCollection AddFastEndpointsServices(this IServiceCollection services)
    {
        services.AddFastEndpoints().SwaggerDocument();
        return services;
    }

    private static IServiceCollection AddDbContextServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = new SqliteConnection("DataSource=myshareddb;mode=memory;cache=shared");
        connection.Open();

        services.AddDbContext<ChoreChampDbContext>(options =>
        {
            options.UseSqlite(connection);
        });

        using (var serviceProvider = services.BuildServiceProvider())
        {
            var context = serviceProvider.GetRequiredService<ChoreChampDbContext>();
            context.Database.EnsureCreated();
        }

        return services;
    }

    private static IServiceCollection AddAuthorizationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNames.Admin, policy => policy.RequireRole(RoleNames.Admin));
        });
        return services;
    }

    private static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthenticationCookie(validFor: TimeSpan.FromMinutes(10));
        return services;
    }

    private static IServiceCollection AddPasswordService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordService, PasswordService>();
        return services;
    }
}
