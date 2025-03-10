namespace ChoreChamp.API.Infrastructure.DependencyInjection;

public static class OpenApiRegistration
{
    public static IServiceCollection AddOpenApiServices(this IServiceCollection services)
    {
        services.AddOpenApi();
        return services;
    }   
}
