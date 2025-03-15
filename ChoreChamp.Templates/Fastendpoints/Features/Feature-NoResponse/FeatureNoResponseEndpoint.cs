using ChoreChamp.API.Infrastructure.Persistence;
using FastEndpoints;

namespace ChoreChamp.API.Templates.Features.Feature;

public class FeatureNoResponseEndpoint(IChoreChampDbContext dbContext)
    : Ep.Req<FeatureNoResponseRequest>.NoRes
{
    public override void Configure() { }

    public override async Task HandleAsync(FeatureNoResponseRequest r, CancellationToken c) { }
}
