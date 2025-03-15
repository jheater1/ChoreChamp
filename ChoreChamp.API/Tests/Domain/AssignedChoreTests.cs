using ChoreChamp.API.Domain;
using Xunit;

namespace ChoreChamp.API.Tests.Domain;

public class AssignedChoreTests
{
    [Fact]
    public void MarkCompleted_ShouldSetIsCompletedToTrue()
    {
        // Arrange
        var assignedChore = new AssignedChore(1, 1, DateTime.Now);
        // Act
        assignedChore.MarkCompleted();
        // Assert
        Assert.True(assignedChore.IsCompleted);
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
        Assert.True(assignedChore.IsApproved);
    }

    [Fact]
    public void Approve_WhenChoreIsNotCompleted_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var assignedChore = new AssignedChore(1, 1, DateTime.Now);
        // Act
        void action() => assignedChore.Approve();
        // Assert
        Assert.Throws<InvalidOperationException>(action);
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
        Assert.False(assignedChore.IsCompleted);
    }

    [Fact]
    public void Reject_WhenChoreIsNotCompleted_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var assignedChore = new AssignedChore(1, 1, DateTime.Now);
        // Act
        void action() => assignedChore.Reject();
        // Assert
        Assert.Throws<InvalidOperationException>(action);
    }

    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange
        var userId = 1;
        var choreId = 1;
        var dueDate = DateTime.Now;
        // Act
        var assignedChore = new AssignedChore(userId, choreId, dueDate);
        // Assert
        Assert.Equal(userId, assignedChore.UserId);
        Assert.Equal(choreId, assignedChore.ChoreId);
        Assert.Equal(dueDate, assignedChore.DueDate);
        Assert.False(assignedChore.IsCompleted);
        Assert.False(assignedChore.IsApproved);
    }
}
