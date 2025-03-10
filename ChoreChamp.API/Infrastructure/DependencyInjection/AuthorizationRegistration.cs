namespace ChoreChamp.API.Infrastructure.DependencyInjection;

public static class AuthorizationRegistration
{
    public static IServiceCollection AddAuthorizationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization();
        return services;
    }
}
