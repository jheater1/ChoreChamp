using System.Security.Claims;
using System.Security.Claims;
using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Infrastructure.Security;
using ChoreChamp.API.Shared.Constants;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Features.Auth.ChangePassword;

public class ChangePasswordEndpoint(ChoreChampDbContext dbContext, IPasswordService passwordService)
    : Ep.Req<ChangePasswordRequest>.NoRes
{
    public override void Configure()
    {
        Post(ApiRoutes.Auth.ChangePassword);
        Permissions(PermissionNames.ChangePassword);
    }

    public override async Task HandleAsync(ChangePasswordRequest request, CancellationToken c)
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null)
        {
            await SendUnauthorizedAsync();
            return;
        }

        if (request.NewPassword != request.ConfirmNewPassword)
        {
            AddError("Passwords do not match");
            await SendErrorsAsync(400, c);
            return;
        }

        var user = await dbContext
            .Users.Where(user => user.Email == userEmail)
            .FirstOrDefaultAsync(c);

        if (user == null || !passwordService.VerifyPassword(request.Password, user.PasswordHash))
        {
            await SendUnauthorizedAsync();
            return;
        }

        var newHashedPassword = passwordService.HashPassword(request.NewPassword);

        await dbContext.Users.ExecuteUpdateAsync(setters =>
            setters.SetProperty(u => u.PasswordHash, newHashedPassword)
        );

        await SendNoContentAsync();
    }
}
