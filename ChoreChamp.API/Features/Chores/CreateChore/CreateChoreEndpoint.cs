﻿using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Shared.Constants;
using FastEndpoints;

namespace ChoreChamp.API.Features.Chores.CreateChore;

public class CreateChoreEndpoint(ChoreChampDbContext dbContext)
    : Ep.Req<CreateChoreRequest>.Res<CreateChoreResponse>.Map<CreateChoreMapper>
{
    public override void Configure()
    {
        Post(ApiRoutes.Chores.Base);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateChoreRequest r, CancellationToken c)
    {
        var chore = Map.ToEntity(r);
        dbContext.Chores.Add(chore);
        await dbContext.SaveChangesAsync(c);
        var response = Map.FromEntity(chore);
        await SendCreatedAtAsync<CreateChoreEndpoint>(new { id = chore.Id }, response);
    }
}
