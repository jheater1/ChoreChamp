using ChoreChamp.API.Domain;
using FastEndpoints;

namespace ChoreChamp.API.Features.Chores.CreateChore;

public class CreateChoreMapper : Mapper<CreateChoreRequest, CreateChoreResponse, Chore>
{
    public override Chore ToEntity(CreateChoreRequest r) =>
        new()
        {
            Name = r.Name,
            Description = r.Description,
            Points = r.Points,
        };

    public override CreateChoreResponse FromEntity(Chore e) =>
        new CreateChoreResponse(e.Id, e.Name, e.Description, e.Points);
}
