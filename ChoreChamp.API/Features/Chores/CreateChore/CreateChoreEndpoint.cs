using ChoreChamp.API.Domain;
using ChoreChamp.API.Infrastructure.Persistence;
using FastEndpoints;

namespace ChoreChamp.API.Features.Chores.CreateChore;

public class CreateChoreEndpoint(ChoreChampDbContext dbContext) : 
    Ep.Req<CreateChoreRequest>.Res<CreateChoreResponse>.Map<CreateChoreMapper> 
{
    public override void Configure()
    {
        Post("api/chores");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateChoreRequest r, CancellationToken c)
    {
        var chore = Map.ToEntity(r);
        dbContext.Chores.Add(chore);
        await dbContext.SaveChangesAsync(c);
        Response = Map.FromEntity(chore);
        await SendCreatedAtAsync<CreateChoreEndpoint>(new {id = chore.Id}, Response);
    }
}
