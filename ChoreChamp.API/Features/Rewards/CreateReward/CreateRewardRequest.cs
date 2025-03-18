namespace ChoreChamp.API.Features.Rewards.CreateReward;

public record CreateRewardRequest(string Name, string? Description, int PointCost, int? Limit);
