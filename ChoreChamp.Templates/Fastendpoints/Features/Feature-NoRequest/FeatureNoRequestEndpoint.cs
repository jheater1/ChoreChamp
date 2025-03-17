using ChoreChamp.API.Infrastructure.Persistence;
using FastEndpoints;

namespace ChoreChamp.Templates.Features.FeatureNoRequest;

public class FeatureNoRequestEndpoint(IChoreChampDbContext dbContext)
    : Ep.NoReq.Res<FeatureNoRequestResponse>.Map<FeatureNoRequestMapper>
{
    public override void Configure() { }

    public override async Task HandleAsync(CancellationToken c) { }
}
