using ChoreChamp.API.Infrastructure.Security;

namespace ChoreChamp.API.Features.Auth.Login;

public class RolePermissionService : IRolePermissionService
{
    private readonly Dictionary<string, IEnumerable<string>> _rolePermissions = new()
    {
        { RoleNames.Admin, new List<string> { PermissionNames.ChangePassword } },
        { RoleNames.User, new List<string> { PermissionNames.ChangePassword } }
    };

    public IEnumerable<string> GetPermissionsForRoles(string role)
    {
        return _rolePermissions.GetValueOrDefault(role) ?? Enumerable.Empty<string>();
    }
}
