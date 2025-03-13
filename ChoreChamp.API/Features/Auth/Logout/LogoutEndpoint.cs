using ChoreChamp.API.Shared.Constants;
using FastEndpoints;
using FastEndpoints.Security;

namespace ChoreChamp.API.Features.Auth.Logout;

public class LogoutEndpoint : Ep.NoReq.NoRes
{
    public override void Configure()
    {
        Post(ApiRoutes.Auth.Logout);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        await CookieAuth.SignOutAsync();
        await SendNoContentAsync(c);
    }
}
