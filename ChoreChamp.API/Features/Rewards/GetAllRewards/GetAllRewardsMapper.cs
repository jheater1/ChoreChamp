using ChoreChamp.API.Domain;
using FastEndpoints;

namespace ChoreChamp.API.Features.Rewards.GetAllRewards;

public class GetAllRewardsMapper
    : ResponseMapper<IEnumerable<GetAllRewardsResponse>, IEnumerable<Reward>>
{
    public override IEnumerable<GetAllRewardsResponse> FromEntity(IEnumerable<Reward> entities) =>
        entities.Select(e => new GetAllRewardsResponse(
            e.Id,
            e.Name,
            e.Description,
            e.PointCost,
            e.Limit,
            e.IsAvailable
        ));
}
