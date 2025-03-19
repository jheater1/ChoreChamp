using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Shared.Constants;
using FastEndpoints;

namespace ChoreChamp.API.Features.Rewards.GetRewardById;

public class GetRewardByIdEndpoint(IChoreChampDbContext dbContext)
    : Ep.Req<GetRewardByIdRequest>.Res<GetRewardByIdResponse>.Map<GetRewardByIdMapper>
{
    public override void Configure()
    {
        Get(ApiRoutes.Rewards.ById);
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetRewardByIdRequest r, CancellationToken c)
    {
        var reward = await dbContext.Rewards.FindAsync(r.Id, c);

        if (reward == null)
        {
            await SendNotFoundAsync(c);
            return;
        }

        Response = Map.FromEntity(reward);
    }
}
