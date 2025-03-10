namespace ChoreChamp.API.Infrastructure.ApplicationConfiguration;

public static class OpenApiConfiguration
{
    public static void ConfigureOpenApi(this WebApplication app)
    {
        app.MapOpenApi();
    }
}
