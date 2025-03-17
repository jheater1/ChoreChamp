using ChoreChamp.API.Domain;
using FluentAssertions;
using Xunit;

namespace ChoreChamp.Test.UnitTests.Domain.AssignedChores;

public class AssignedChoreTests
{
    [Fact]
    public void CreateAssignedChore_WithValidParameters_ShouldCreateAssignedChore()
    {
        // Arrange
        var userId = 1;
        var choreId = 1;
        var dueDate = DateTime.Now;
        // Act
        var assignedChore = new AssignedChore(userId, choreId, dueDate);
        // Assert
        assignedChore.UserId.Should().Be(userId);
        assignedChore.ChoreId.Should().Be(choreId);
        assignedChore.DueDate.Should().Be(dueDate);
        assignedChore.IsCompleted.Should().BeFalse();
        assignedChore.IsApproved.Should().BeFalse();
    }

    [Fact]
    public void MarkCompleted_ShouldSetIsCompletedToTrue()
    {
        // Arrange
        var assignedChore = new AssignedChore(1, 1, DateTime.Now);
        // Act
        assignedChore.MarkCompleted();
        // Assert
        assignedChore.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public void Approve_WhenChoreIsCompleted_ShouldSetIsApprovedToTrue()
    {
        // Arrange
        var assignedChore = new AssignedChore(1, 1, DateTime.Now);
        assignedChore.MarkCompleted();
        // Act
        assignedChore.Approve();
        // Assert
        assignedChore.IsApproved.Should().BeTrue();
    }

    [Fact]
    public void Approve_WhenChoreIsNotCompleted_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var assignedChore = new AssignedChore(1, 1, DateTime.Now);
        // Act
        Action act = () => assignedChore.Approve();
        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Chore must be completed before approving.");
    }

    [Fact]
    public void Reject_WhenChoreIsCompleted_ShouldSetIsCompletedToFalse()
    {
        // Arrange
        var assignedChore = new AssignedChore(1, 1, DateTime.Now);
        assignedChore.MarkCompleted();
        // Act
        assignedChore.Reject();
        // Assert
        assignedChore.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public void Reject_WhenChoreIsNotCompleted_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var assignedChore = new AssignedChore(1, 1, DateTime.Now);
        // Act
        var act = () => assignedChore.Reject();
        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Chore must be completed before rejecting.");
    }
}
