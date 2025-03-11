namespace ChoreChamp.API.Shared.Constants;

public static class RouteSegments
{
    public const string Id = "/{id}";
}

public static class ApiRoutes
{
    public static class Auth
    {
        public const string Base = "auth";
        public const string Login = Base + "/login";
        public const string Logout = Base + "/logout";
    }

    public static class Chores
    {
        public const string Base = "chores";
        public const string ById = Base + RouteSegments.Id;
    }

    public static class Users
    {
        public const string Base = "users";
    }
}
