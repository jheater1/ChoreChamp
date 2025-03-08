namespace ChoreChamp.API.Infrastructure.DependencyInjection;

public static class MediaRServiceRegistration
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        return services;
    }
}
