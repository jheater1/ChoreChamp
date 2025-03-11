using ChoreChamp.API.Domain;
using FastEndpoints;

namespace ChoreChamp.API.Features.Chores.GetChorebyId;

public class GetChoreByIdMapper : ResponseMapper<GetChoreByIdResponse, Chore>
{
    public override GetChoreByIdResponse FromEntity(Chore e) =>
        new GetChoreByIdResponse(e.Id, e.Name, e.Description, e.Points);
}
