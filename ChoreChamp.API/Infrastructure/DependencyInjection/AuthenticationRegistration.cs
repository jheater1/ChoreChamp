using FastEndpoints.Security;

namespace ChoreChamp.API.Infrastructure.DependencyInjection;

public static class AuthenticationRegistration
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthenticationCookie(validFor: TimeSpan.FromMinutes(10));
        return services;
    }
}
