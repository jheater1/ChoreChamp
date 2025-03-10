namespace ChoreChamp.API.Infrastructure.ApplicationConfiguration;

public static class AuthorizationConfiguration
{
    public static void ConfigureAuthorization(this WebApplication app)
    {
        app.UseAuthorization();
    }
}
