using ChoreChamp.API.Features.Auth.ChangePassword;
using FluentValidation.TestHelper;
using Xunit;

namespace ChoreChamp.Test.UnitTests.Features.Auth.ChangePassword
{
    public class ChangePasswordValidatorTests
    {
        private readonly ChangePasswordValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Password_Is_Empty()
        {
            // Arrange
            var model = new ChangePasswordRequest("", "newpassword", "newpassword");

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Have_Error_When_NewPassword_Is_Empty()
        {
            var model = new ChangePasswordRequest("oldpassword", "", "newpassword");
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public void Should_Have_Error_When_NewPassword_Is_Short()
        {
            var model = new ChangePasswordRequest("oldpassword", "short", "short");
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public void Should_Have_Error_When_ConfirmNewPassword_Is_Empty()
        {
            var model = new ChangePasswordRequest("oldpassword", "newpassword", "");
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ConfirmNewPassword);
        }

        [Fact]
        public void Should_Have_Error_When_ConfirmNewPassword_Is_Short()
        {
            var model = new ChangePasswordRequest("oldpassword", "newpassword", "short");
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ConfirmNewPassword);
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Model()
        {
            var model = new ChangePasswordRequest("oldpassword", "newpassword", "newpassword");
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
