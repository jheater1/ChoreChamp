using System.Security.Claims;
using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Infrastructure.Security;
using ChoreChamp.API.Shared.Constants;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Features.Auth.Login;

public class LoginEndpoint(
    ChoreChampDbContext dbContext,
    IPasswordService passwordService,
    IRolePermissionService rolePermissionService
) : Ep.Req<LoginRequest>.NoRes
{
    public override void Configure()
    {
        Post(ApiRoutes.Auth.Login);
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest request, CancellationToken c)
    {
        var user = await dbContext
            .Users.Where(user => user.Email == request.Email)
            .FirstOrDefaultAsync(c);

        if (user == null || !passwordService.VerifyPassword(request.Password, user.PasswordHash))
        {
            await SendUnauthorizedAsync();
            return;
        }

        await CookieAuth.SignInAsync(u =>
        {
            u.Roles.Add(user.IsAdmin ? RoleNames.Admin : RoleNames.User);
            var permissions = rolePermissionService.GetPermissionsForRoles(u.Roles.First());

            foreach (var permission in permissions)
            {
                u.Permissions.Add(permission);
            }

            u[ClaimTypes.Email] = user.Email;
        });

        await SendNoContentAsync();
    }
}
