using FluentValidation;

namespace ChoreChamp.API.Infrastructure.DependencyInjection;

public static class ValidationServiceRegistration
{
    public static IServiceCollection AddValidationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        return services;
    }
}
