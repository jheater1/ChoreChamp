using ChoreChamp.API.Domain;
using ChoreChamp.API.Infrastructure.Persistence;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ChoreChamp.API.Features.Chores.GetChorebyId;

public class GetChoreByIdEndpoint(ChoreChampDbContext dbContext) :
    Ep.Req<GetChoreByIdRequest>.Res<Results<Ok<GetChoreByIdResponse>, NotFound>>.Map<GetChoreByIdMapper>
{
    public override void Configure()
    {
        Get("api/chores/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetChoreByIdRequest r, CancellationToken c)
    {
        Chore? chore = await dbContext.Chores.FindAsync(r.Id, c);

        if (chore == null)
        {
            await SendNotFoundAsync(c);
            return;
        }

        GetChoreByIdResponse response = Map.FromEntity(chore);

        await SendResultAsync(TypedResults.Ok(response));
    }
}
