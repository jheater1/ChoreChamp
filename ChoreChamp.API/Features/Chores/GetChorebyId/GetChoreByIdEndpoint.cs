using ChoreChamp.API.Domain;
using ChoreChamp.API.Infrastructure.Persistence;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ChoreChamp.API.Features.Chores.GetChorebyId;

public class GetChoreByIdEndpoint(ChoreChampDbContext dbContext) :
    Ep.Req<GetChoreByIdRequest>.Res<GetChoreByIdResponse>.Map<GetChoreByIdMapper>
{
    public override void Configure()
    {
        Get("api/chores/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetChoreByIdRequest r, CancellationToken c)
    {
        var chore = await dbContext.Chores.FindAsync(r.Id, c);

        if (chore == null)
        {
            await SendNotFoundAsync(c);
            return;
        }

        Response = Map.FromEntity(chore);
    }
}
