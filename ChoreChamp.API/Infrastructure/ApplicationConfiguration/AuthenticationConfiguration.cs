namespace ChoreChamp.API.Infrastructure.ApplicationConfiguration;

public static class AuthenticationConfiguration
{
    public static void ConfigureAuthentication(this WebApplication app)
    {
        app.UseAuthentication();
    }
}
