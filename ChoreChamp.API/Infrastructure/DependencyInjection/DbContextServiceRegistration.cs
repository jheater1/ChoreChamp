using ChoreChamp.API.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Infrastructure.DependencyInjection;

public static class DbContextServiceRegistration
{
    public static IServiceCollection AddDbContextServices(this IServiceCollection services, IConfiguration configuration)
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
}
