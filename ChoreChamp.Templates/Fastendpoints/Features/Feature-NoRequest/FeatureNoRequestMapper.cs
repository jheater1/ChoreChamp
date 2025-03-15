using FastEndpoints;

namespace ChoreChamp.API.Templates.Features.Feature;

public class FeatureNoRequestMapper : ResponseMapper<FeatureNoRequestResponse, object>
{
    public override FeatureNoRequestResponse FromEntity(object e) => new();
}
