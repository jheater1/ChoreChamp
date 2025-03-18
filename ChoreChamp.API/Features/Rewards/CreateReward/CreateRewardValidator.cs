using FastEndpoints;
using FluentValidation;

namespace ChoreChamp.API.Features.Rewards.CreateReward;

public class CreateRewardValidator : Validator<CreateRewardRequest>
{
    public CreateRewardValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.PointCost).GreaterThan(0);
        RuleFor(x => x.Limit).GreaterThanOrEqualTo(0);
    }
}
