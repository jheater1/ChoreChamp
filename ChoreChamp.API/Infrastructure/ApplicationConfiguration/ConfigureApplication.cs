namespace ChoreChamp.API.Infrastructure.ApplicationConfiguration;

public static class ConfigureApplication
{
    public static void ConfigureApp(this WebApplication app)
    {
        app.ConfigureFastEndPoints();
        
        if (app.Environment.IsDevelopment())
        {
        
        }
    }
}
