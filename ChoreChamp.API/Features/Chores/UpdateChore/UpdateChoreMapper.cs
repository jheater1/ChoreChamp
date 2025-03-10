using ChoreChamp.API.Domain;
using FastEndpoints;

namespace ChoreChamp.API.Features.Chores.UpdateChore;

public class UpdateChoreMapper : Mapper<UpdateChoreRequest, UpdateChoreResponse, Chore>
{
    public override Chore ToEntity(UpdateChoreRequest r)
    {
        return new()
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            Points = r.Points
        };
    }

    public override UpdateChoreResponse FromEntity(Chore e)
    {
        return new UpdateChoreResponse(e.Id, e.Name, e.Description, e.Points);
    }
}
