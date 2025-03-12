using FastEndpoints;
using FluentValidation;

namespace ChoreChamp.API.Features.Auth.ChangePassword;

public class ChangePasswordValidator : Validator<ChangePasswordRequest>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8);
        RuleFor(x => x.ConfirmNewPassword).NotEmpty().MinimumLength(8);
    }   
}
