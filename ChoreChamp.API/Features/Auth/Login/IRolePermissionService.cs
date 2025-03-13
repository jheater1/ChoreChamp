namespace ChoreChamp.API.Features.Auth.Login;

public interface IRolePermissionService
{
    IEnumerable<string> GetPermissionsForRoles(string role);
}
