using ChoreChamp.API.Infrastructure.Persistence;
using FastEndpoints;

namespace ChoreChamp.API.Templates.Features.Feature;

public class FeatureNoRequestEndpoint(ChoreChampDbContext dbContext)
    : Ep.NoReq.Res<FeatureNoRequestResponse>.Map<FeatureNoRequestMapper>
{
    public override void Configure()
    {

    }

    public override async Task HandleAsync(CancellationToken c)
    {
        
    }
}
