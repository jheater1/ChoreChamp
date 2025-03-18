using ChoreChamp.API.Domain;
using FastEndpoints;

namespace ChoreChamp.API.Features.Rewards.CreateReward;

public class CreateRewardMapper : Mapper<CreateRewardRequest, CreateRewardResponse, Reward>
{
    public override Reward ToEntity(CreateRewardRequest r) =>
        new(r.Name, r.Description, r.PointCost, r.Limit);

    public override CreateRewardResponse FromEntity(Reward e) =>
        new CreateRewardResponse(e.Id, e.Name, e.Description, e.PointCost, e.Limit);
}
