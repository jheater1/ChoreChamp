using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Shared.Constants;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Features.Rewards.GetAllRewards;

public class GetAllRewardsEndpoint(IChoreChampDbContext dbContext)
    : Ep.NoReq.Res<IEnumerable<GetAllRewardsResponse>>.Map<GetAllRewardsMapper>
{
    public override void Configure()
    {
        Get(ApiRoutes.Rewards.Base);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var rewards = await dbContext.Rewards.ToListAsync(c);
        Response = Map.FromEntity(rewards);
    }
}
