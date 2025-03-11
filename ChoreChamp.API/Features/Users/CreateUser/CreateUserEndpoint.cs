using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Infrastructure.Security;
using FastEndpoints;

namespace ChoreChamp.API.Features.Users.CreateUser;

public class CreateUserEndpoint(ChoreChampDbContext dbContext) : Ep.Req<CreateUserRequest>.Res<CreateUserResponse>.Map<CreateUserMapper>
{
    public override void Configure()
    {
        Post("/api/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest r, CancellationToken c)
    {
        var user = Map.ToEntity(r);
        user.PasswordHash = PasswordHasher.HashPassword(r.Password);
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync(c);
        await SendCreatedAtAsync<CreateUserEndpoint>(new { id = user.Id }, Map.FromEntity(user));
    }
}
