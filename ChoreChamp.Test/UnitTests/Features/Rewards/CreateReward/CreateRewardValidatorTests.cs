using ChoreChamp.API.Features.Rewards.CreateReward;
using FluentValidation.TestHelper;

namespace ChoreChamp.Test.UnitTests.Features.Rewards.CreateReward;

public class CreateRewardValidatorTests
{
    private readonly CreateRewardValidator _validator;

    public CreateRewardValidatorTests()
    {
        _validator = new CreateRewardValidator();
    }

    [Fact]
    public void Validate_WhenCalledWithValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = new CreateRewardRequest("New Reward", "Description", 10, null);
        // Act
        var result = _validator.TestValidate(request);
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WhenCalledWithEmptyName_ReturnsError()
    {
        // Arrange
        var request = new CreateRewardRequest(string.Empty, "Description", 10, null);
        // Act
        var result = _validator.TestValidate(request);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WhenCalledWithNegativePointCost_ReturnsError()
    {
        // Arrange
        var request = new CreateRewardRequest("New Reward", "Description", -10, null);
        // Act
        var result = _validator.TestValidate(request);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PointCost);
    }

    [Fact]
    public void Validate_WhenCalledWithLimitLessThanZero_ReturnsError()
    {
        // Arrange
        var request = new CreateRewardRequest("New Reward", "Description", 10, -1);
        // Act
        var result = _validator.TestValidate(request);
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Limit);
    }
}
