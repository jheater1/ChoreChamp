using Scalar.AspNetCore;

namespace ChoreChamp.API.Infrastructure.ApplicationConfiguration;

public static class ScalarConfiguration
{
    public static void ConfigureScalarApiReference(this WebApplication app)
    {
        app.MapScalarApiReference();
    }
}
