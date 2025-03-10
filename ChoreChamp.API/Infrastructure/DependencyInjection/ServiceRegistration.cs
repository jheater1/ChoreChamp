﻿namespace ChoreChamp.API.Infrastructure.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextServices(configuration);
        services.AddFastEndpointsServices();
        services.AddValidationServices();
        services.AddOpenApi();
        return services;
    }
}
