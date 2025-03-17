using FastEndpoints;

namespace ChoreChamp.Templates.Features.FeatureNoRequest;

public class FeatureNoRequestMapper : ResponseMapper<FeatureNoRequestResponse, object>
{
    public override FeatureNoRequestResponse FromEntity(object e) => new();
}
