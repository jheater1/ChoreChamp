using ChoreChamp.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Infrastructure.DependencyInjection;

public static class DbContextServiceRegistration
{
    public static IServiceCollection AddDbContextServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ChoreChampDbContext>(options => 
            options.UseInMemoryDatabase("ChoreChamp"));

        return services;
    }
}
