using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Shared.Constants;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Features.Rewards.GetAllRewards;

public class GetAllAvailableRewardsEndpoint(IChoreChampDbContext dbContext)
    : Ep.NoReq.Res<IEnumerable<GetAllRewardsResponse>>.Map<GetAllRewardsMapper>
{
    public override void Configure()
    {
        Get(ApiRoutes.Rewards.Available);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var rewards = await dbContext.Rewards.Where(x => x.IsAvailable).ToListAsync(c);
        Response = Map.FromEntity(rewards);
    }
}
