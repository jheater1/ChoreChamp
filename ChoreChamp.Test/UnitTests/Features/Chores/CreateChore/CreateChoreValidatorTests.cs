using ChoreChamp.API.Features.Chores.CreateChore;
using FluentValidation.TestHelper;
using Xunit;

namespace ChoreChamp.Test.UnitTests.Features.Chores.CreateChore
{
    public class CreateChoreValidatorTests
    {
        private readonly CreateChoreValidator _validator;

        public CreateChoreValidatorTests()
        {
            _validator = new CreateChoreValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            // Arrange
            var model = new CreateChoreRequest(string.Empty, "A valid description", 5);

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Exceeds_MaxLength()
        {
            // Arrange
            var model = new CreateChoreRequest(new string('A', 51), "A valid description", 5);

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Empty()
        {
            // Arrange
            var model = new CreateChoreRequest("Valid Name", string.Empty, 5);

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Exceeds_MaxLength()
        {
            // Arrange
            var model = new CreateChoreRequest("Valid Name", new string('B', 201), 5);

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Points_Is_Less_Than_One()
        {
            // Arrange
            var model = new CreateChoreRequest("Valid Name", "Valid Description", 0);

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Points);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
        {
            // Arrange
            var model = new CreateChoreRequest("Valid Name", "Valid Description", 5);

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
