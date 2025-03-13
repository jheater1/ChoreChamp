using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Shared.Constants;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Features.Chores.UpdateChore;

public class UpdateChoreEndpoint(ChoreChampDbContext dbContext)
    : Ep.Req<UpdateChoreRequest>.Res<UpdateChoreResponse>.Map<UpdateChoreMapper>
{
    public override void Configure()
    {
        Put(ApiRoutes.Chores.Base);
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateChoreRequest r, CancellationToken c)
    {
        var choreExists = await dbContext.Chores.AnyAsync(x => x.Id == r.Id, c);

        if (!choreExists)
        {
            await SendNotFoundAsync(c);
            return;
        }

        var chore = Map.ToEntity(r);
        dbContext.Update(chore);
        await dbContext.SaveChangesAsync(c);
        Response = Map.FromEntity(chore);
    }
}
