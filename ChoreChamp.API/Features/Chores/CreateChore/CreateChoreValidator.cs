using FastEndpoints;
using FluentValidation;

namespace ChoreChamp.API.Features.Chores.CreateChore;

public class CreateChoreValidator : Validator<CreateChoreRequest>
{
    public CreateChoreValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Points).NotEmpty().GreaterThanOrEqualTo(1);
    }
}
