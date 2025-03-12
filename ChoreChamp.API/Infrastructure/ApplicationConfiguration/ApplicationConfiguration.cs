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

        if (app.Environment.IsDevelopment()) { }
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
