using ChoreChamp.API.Domain;
using FastEndpoints;

namespace ChoreChamp.API.Features.Rewards.GetRewardById;

public class GetRewardByIdMapper : Mapper<GetRewardByIdRequest, GetRewardByIdResponse, Reward>
{
    public override GetRewardByIdResponse FromEntity(Reward e) =>
        new(e.Id, e.Name, e.Description, e.PointCost, e.Limit, e.IsAvailable);
}
