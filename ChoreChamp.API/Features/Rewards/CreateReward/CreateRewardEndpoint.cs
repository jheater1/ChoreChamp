using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Shared.Constants;
using FastEndpoints;

namespace ChoreChamp.API.Features.Rewards.CreateReward;

public class CreateRewardEndpoint(IChoreChampDbContext dbContext)
    : Ep.Req<CreateRewardRequest>.Res<CreateRewardResponse>.Map<CreateRewardMapper>
{
    public override void Configure()
    {
        Post(ApiRoutes.Rewards.Base);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateRewardRequest r, CancellationToken c)
    {
        var entity = Map.ToEntity(r);
        dbContext.Rewards.Add(entity);
        await dbContext.SaveChangesAsync(c);
        Response = Map.FromEntity(entity);
    }
}
