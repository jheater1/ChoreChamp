using ChoreChamp.API.Domain;
using FastEndpoints;

namespace ChoreChamp.API.Features.Chores.GetAllChores;

public class GetAllChoresMapper
    : ResponseMapper<IEnumerable<GetAllChoresResponse>, IEnumerable<Chore>>
{
    public override IEnumerable<GetAllChoresResponse> FromEntity(IEnumerable<Chore> entities) =>
        entities.Select(e => new GetAllChoresResponse(e.Id, e.Name, e.Description, e.Points));
}
