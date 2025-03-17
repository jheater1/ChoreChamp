using ChoreChamp.API.Domain;
using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Infrastructure.Security;
using ChoreChamp.API.Shared.Constants;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Features.Users.CreateUser;

public class CreateUserEndpoint(IChoreChampDbContext dbContext, IPasswordService passwordService)
    : Ep.Req<CreateUserRequest>.Res<CreateUserResponse>.Map<CreateUserMapper>
{
    public override void Configure()
    {
        Post(ApiRoutes.Users.Base);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest r, CancellationToken c)
    {
        var user = new User(r.Name, r.Email, r.Password, r.IsAdmin, passwordService);

        bool userExists = await dbContext.Users.AnyAsync(x => x.Email == r.Email, c);
        if (userExists)
        {
            AddError("A user with this email already exists");
            await SendErrorsAsync(409, c);
            return;
        }

        await dbContext.Users.AddAsync(user, c);
        await dbContext.SaveChangesAsync(c);
        await SendCreatedAtAsync<CreateUserEndpoint>(new { id = user.Id }, Map.FromEntity(user));
    }
}
