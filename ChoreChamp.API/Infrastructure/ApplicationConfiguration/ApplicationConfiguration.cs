using ChoreChamp.API.Infrastructure.Seeder;
using FastEndpoints;
using FastEndpoints.Swagger;

namespace ChoreChamp.API.Infrastructure.ApplicationConfiguration;

public static class ConfigureApplication
{
    public static void Configure(this WebApplication app)
    {
        app.ConfigureFastEndPoints();
        app.ConfigureAuthentication();
        app.ConfigureAuthorization();

        if (app.Environment.IsDevelopment())
        {
            SeedDatabase(app);
        }
    }

    private static void SeedDatabase(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DevDataSeeder>();
        seeder.SeedAsync().GetAwaiter().GetResult();
    }

    private static void ConfigureAuthentication(this WebApplication app)
    {
        app.UseAuthentication();
    }
    private static void ConfigureAuthorization(this WebApplication app)
    {
        app.UseAuthorization();
    }

    public static void ConfigureFastEndPoints(this WebApplication app)
    {
        app.UseFastEndpoints().UseSwaggerGen();
    }
}
