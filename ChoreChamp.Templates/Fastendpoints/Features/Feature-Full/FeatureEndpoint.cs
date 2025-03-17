using ChoreChamp.API.Infrastructure.Persistence;
using FastEndpoints;

namespace ChoreChamp.Templates.Features.FeatureFull;

public class FeatureEndpoint(IChoreChampDbContext dbContext)
    : Ep.Req<FeatureRequest>.Res<FeatureResponse>.Map<FeatureMapper>
{
    public override void Configure() { }

    public override async Task HandleAsync(FeatureRequest r, CancellationToken c)
    {
        var entity = Map.ToEntity(r);
        // dbContext operation
        await dbContext.SaveChangesAsync();
        Response = Map.FromEntity(entity);
    }
}
