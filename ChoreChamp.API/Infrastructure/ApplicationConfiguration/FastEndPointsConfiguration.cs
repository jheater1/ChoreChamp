using FastEndpoints;

namespace ChoreChamp.API.Infrastructure.ApplicationConfiguration;

public static class FastEndPointsConfiguration
{
    public static void ConfigureFastEndPoints(this WebApplication app)
    {
        app.UseFastEndpoints();
    }
}
