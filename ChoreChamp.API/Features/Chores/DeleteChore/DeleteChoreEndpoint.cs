using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Shared.Constants;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Features.Chores.DeleteChore;

public class DeleteChoreEndpoint(IChoreChampDbContext dbContext) : Ep.Req<DeleteChoreRequest>.NoRes
{
    public override void Configure()
    {
        Delete(ApiRoutes.Chores.ById);
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteChoreRequest r, CancellationToken c)
    {
        var deleted = await dbContext.Chores.Where(e => e.Id == r.Id).FirstOrDefaultAsync(c);

        if (deleted == null)
        {
            await SendNotFoundAsync(c);
            return;
        }

        dbContext.Chores.Remove(deleted);
        await dbContext.SaveChangesAsync(c);

        await SendNoContentAsync();
    }
}
