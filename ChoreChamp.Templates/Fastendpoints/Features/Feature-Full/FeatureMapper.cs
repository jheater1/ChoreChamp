using FastEndpoints;

namespace ChoreChamp.Templates.Features.FeatureFull;

public class FeatureMapper : Mapper<FeatureRequest, FeatureResponse, object>
{
    public override object ToEntity(FeatureRequest r) => new();

    public override FeatureResponse FromEntity(object e) => new();
}
