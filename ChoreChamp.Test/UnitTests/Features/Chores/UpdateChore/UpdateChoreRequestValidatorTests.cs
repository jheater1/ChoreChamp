using ChoreChamp.API.Features.Chores.UpdateChore;
using FluentValidation.TestHelper;

namespace ChoreChamp.Test.UnitTests.Features.Chores.UpdateChore
{
    public class UpdateChoreRequestValidatorTests
    {
        private readonly UpdateChoreValidator _validator;

        public UpdateChoreRequestValidatorTests()
        {
            _validator = new UpdateChoreValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            // Arrange
            var model = new UpdateChoreRequest(1, string.Empty, "A valid description", 5);

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Exceeds_MaxLength()
        {
            // Arrange
            var model = new UpdateChoreRequest(1, new string('A', 51), "A valid description", 5);

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Empty()
        {
            // Arrange
            var model = new UpdateChoreRequest(1, "Valid Name", string.Empty, 5);

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Exceeds_MaxLength()
        {
            // Arrange
            var model = new UpdateChoreRequest(1, "Valid Name", new string('B', 201), 5);

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Points_Is_Less_Than_One()
        {
            // Arrange
            var model = new UpdateChoreRequest(1, "Valid Name", "Valid Description", 0); // Invalid since Points must be >= 1

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Points);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
        {
            // Arrange
            var model = new UpdateChoreRequest(1, "Valid Name", "Valid Description", 5);

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
