using ChoreChamp.API.Domain;
using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Shared.Constants;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ChoreChamp.API.Features.Rewards.GetAllRewards;

public class GetAllRewardsEndpoint(IChoreChampDbContext dbContext)
    : Ep.Req<GetAllRewardsRequest>.Res<IEnumerable<GetAllRewardsResponse>>.Map<GetAllRewardsMapper>
{
    public override void Configure()
    {
        Get(ApiRoutes.Rewards.Base);
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetAllRewardsRequest r, CancellationToken c)
    {
        var rewards = new List<Reward>();

        if (r.AllAvailable is not null && r.AllAvailable.Value)
        {
            rewards = await dbContext.Rewards.Where(r => r.IsAvailable).ToListAsync(c);
            Response = Map.FromEntity(rewards);
            return;
        }

        rewards = await dbContext.Rewards.ToListAsync(c);

        Response = Map.FromEntity(rewards);
    }
}
