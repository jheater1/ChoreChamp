using ChoreChamp.API.Domain;
using FluentAssertions;

namespace ChoreChamp.Test.UnitTests.Domain.Rewards;

public class RewardTests
{
    [Fact]
    public void CreateReward_WithValidParameters_CreatesReward()
    {
        // Arrange
        var name = "Test Reward";
        var description = "Test Description";
        var pointCost = 100;
        var limit = 10;
        // Act
        var reward = new Reward(name, description, pointCost, limit);
        // Assert
        reward.Name.Should().Be(name);
        reward.Description.Should().Be(description);
        reward.PointCost.Should().Be(pointCost);
        reward.Limit.Should().Be(limit);
        reward.IsAvailable.Should().BeTrue();
    }

    [Fact]
    public void CreateReward_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        var name = "";
        var description = "Test Description";
        var pointCost = 100;
        var limit = 10;
        // Act
        Action act = () => new Reward(name, description, pointCost, limit);
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Name cannot be null or whitespace.");
    }

    [Fact]
    public void CreateReward_WithNegativePointCost_ThrowsArgumentException()
    {
        // Arrange
        var name = "Test Reward";
        var description = "Test Description";
        var pointCost = -100;
        var limit = 10;
        // Act
        Action act = () => new Reward(name, description, pointCost, limit);
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Point cost cannot be negative.");
    }

    [Fact]
    public void CreateReward_WithNegativeLimit_ThrowsArgumentException()
    {
        // Arrange
        var name = "Test Reward";
        var description = "Test Description";
        var pointCost = 100;
        var limit = -10;
        // Act
        Action act = () => new Reward(name, description, pointCost, limit);
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Limit cannot be negative.");
    }

    [Fact]
    public void UpdateAvailability_TogglesIsAvailable()
    {
        // Arrange
        var reward = new Reward("Test Reward", "Test Description", 100, 10);
        // Act
        reward.UpdateAvailability();
        // Assert
        reward.IsAvailable.Should().BeFalse();
    }

    [Fact]
    public void UpdateLimit_WithValidLimit_UpdatesLimit()
    {
        // Arrange
        var reward = new Reward("Test Reward", "Test Description", 100, 10);
        var newLimit = 5;
        // Act
        reward.UpdateLimit(newLimit);
        // Assert
        reward.Limit.Should().Be(newLimit);
    }

    [Fact]
    public void UpdateLimit_WithNegativeLimit_ThrowsArgumentException()
    {
        // Arrange
        var reward = new Reward("Test Reward", "Test Description", 100, 10);
        var newLimit = -5;
        // Act
        Action act = () => reward.UpdateLimit(newLimit);
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Limit cannot be negative.");
    }

    [Fact]
    public void UpdateLimit_WithZeroLimit_UpdatesLimit()
    {
        // Arrange
        var reward = new Reward("Test Reward", "Test Description", 100, 10);
        var newLimit = 0;
        // Act
        reward.UpdateLimit(newLimit);
        // Assert
        reward.Limit.Should().Be(newLimit);
    }
}
