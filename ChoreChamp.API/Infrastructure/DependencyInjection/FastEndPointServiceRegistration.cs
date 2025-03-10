using FastEndpoints;

namespace ChoreChamp.API.Infrastructure.DependencyInjection;

public static class FastEndPointServiceRegistration
{
    public static IServiceCollection AddFastEndpointsServices(this IServiceCollection services)
    {
        services.AddFastEndpoints();
        return services;
    }
}
