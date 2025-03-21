﻿using ChoreChamp.API.Features.Auth.Login;
using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Infrastructure.Persistence.Seeder;
using ChoreChamp.API.Infrastructure.Security;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ChoreChamp.API.Infrastructure.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContextServices(configuration);
        services.AddFastEndpointsServices();
        services.AddValidationServices();
        services.AddAuthenticationServices();
        services.AddAuthorizationServices();
        services.AddPasswordService();
        services.AddRolePermissionService();

        if (configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            services.AddDataSeeder();
        }
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

    private static IServiceCollection AddDbContextServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connection = new SqliteConnection(configuration.GetConnectionString("SqlLiteInMemory"));
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

        services.AddScoped<IChoreChampDbContext, ChoreChampDbContext>();

        return services;
    }

    private static IServiceCollection AddAuthorizationServices(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNames.Admin, policy => policy.RequireRole(RoleNames.Admin));
        });
        return services;
    }

    private static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddAuthenticationCookie(validFor: TimeSpan.FromMinutes(10));
        return services;
    }

    private static IServiceCollection AddPasswordService(this IServiceCollection services)
    {
        services.AddScoped<IPasswordService, PasswordService>();
        return services;
    }

    private static IServiceCollection AddRolePermissionService(this IServiceCollection services)
    {
        services.AddScoped<IRolePermissionService, RolePermissionService>();
        return services;
    }

    private static IServiceCollection AddDataSeeder(this IServiceCollection services)
    {
        services.AddScoped<DevDataSeeder>();
        return services;
    }
}
