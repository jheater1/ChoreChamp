namespace ChoreChamp.API.Features.Rewards.GetRewardById;

public record GetRewardByIdResponse(
    int Id,
    string Name,
    string? Description,
    int PointCost,
    int? Limit,
    bool IsAvailable
);
