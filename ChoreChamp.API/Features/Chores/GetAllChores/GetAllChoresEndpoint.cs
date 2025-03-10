using ChoreChamp.API.Infrastructure.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Features.Chores.GetAllChores;

public class GetAllChoresEndpoint(ChoreChampDbContext dbContext) : 
    Ep.NoReq.Res<IEnumerable<GetAllChoresResponse>>.Map<GetAllChoresMapper>
{
    public override void Configure()
    {
        Get("api/chores");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var chores = await dbContext.Chores.ToListAsync(c);
        Response = Map.FromEntity(chores);
        await SendAsync(Response);
    }
}
