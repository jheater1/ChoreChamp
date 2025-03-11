using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Infrastructure.Security;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Features.Users.CreateUser;

public class CreateUserEndpoint(ChoreChampDbContext dbContext, IPasswordService passwordService) :
    Ep.Req<CreateUserRequest>.Res<CreateUserResponse>.Map<CreateUserMapper>
{
    public override void Configure()
    {
        Post("/api/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest r, CancellationToken c)
    {
        var user = Map.ToEntity(r);

        bool userExists = await dbContext.Users.AnyAsync(x => x.Email == r.Email, c);
        if (userExists)
        {
            AddError("A user with this email already exists");
            await SendErrorsAsync(409, cancellation: c);
            return;
        }

        user.PasswordHash = passwordService.HashPassword(r.Password);
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync(c);
        await SendCreatedAtAsync<CreateUserEndpoint>(new { id = user.Id }, Map.FromEntity(user));
    }
}
