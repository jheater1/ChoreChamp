namespace ChoreChamp.API.Features.Rewards.CreateReward;

public record CreateRewardResponse(
    int Id,
    string Name,
    string? Description,
    int PointCost,
    int? Limit
);
