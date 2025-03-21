﻿namespace ChoreChamp.API.Features.Rewards.GetAllRewards;

public record GetAllRewardsResponse(
    int Id,
    string Name,
    string? Description,
    int PointCost,
    int? Limit,
    bool IsAvailable
);
