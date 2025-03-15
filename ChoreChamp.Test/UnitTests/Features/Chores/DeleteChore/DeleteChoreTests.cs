using ChoreChamp.API.Domain;
using ChoreChamp.API.Features.Chores.DeleteChore;
using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.Test.UnitTests.Features.Chores.Utilities;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable.Moq;
using Moq;

namespace ChoreChamp.Test.UnitTests.Features.Chores.DeleteChore
{
    public class DeleteChoreTests
    {
        [Fact]
        public async Task DeleteChore_WhenCalledWithExistingChore_DeletesChore()
        {
            // Arrange: Create a list with 1 chore.
            var chores = ChoreTestDataFactory.CreateChores(1).ToList();
            var existingChore = chores.First();

            // Build a mock DbSet that wraps the underlying list.
            var mockChoreDbSet = chores.AsQueryable().BuildMockDbSet();
            // Setup Remove so that it removes the chore from the list.
            mockChoreDbSet
                .Setup(d => d.Remove(It.IsAny<Chore>()))
                .Callback<Chore>(chore => chores.Remove(chore))
                .Returns((Chore chore) => null);

            // Create a mock IApplicationDbContext and setup its Set<Chore>() method.
            var dbContextMock = new Mock<IChoreChampDbContext>();
            dbContextMock.Setup(x => x.Chores).Returns(mockChoreDbSet.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            var endpoint = Factory.Create<DeleteChoreEndpoint>(dbContextMock.Object);
            var request = new DeleteChoreRequest(existingChore.Id);

            // Act
            await endpoint.HandleAsync(request, default);
            var response = endpoint.Response;

            // Assert: Verify that the underlying "database" (the list) is now empty
            // and the endpoint response is null (adjust based on your endpoint behavior).
            chores.Should().BeEmpty();
            response.Should().BeNull();
        }

        [Fact]
        public async Task DeleteChore_WhenCalledWithNonExistingChore_ReturnsNotFound()
        {
            // Arrange: Create an empty list of chores.
            var chores = ChoreTestDataFactory.CreateEmptyChores().ToList();

            // Build a mock DbSet from the empty list.
            var mockChoreDbSet = chores.AsQueryable().BuildMockDbSet();
            // Setup Remove in case it's called (it shouldn't be in this test).
            mockChoreDbSet
                .Setup(d => d.Remove(It.IsAny<Chore>()))
                .Callback<Chore>(chore => chores.Remove(chore))
                .Returns((Chore chore) => null);

            var dbContextMock = new Mock<IChoreChampDbContext>();
            dbContextMock.Setup(x => x.Chores).Returns(mockChoreDbSet.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(0);

            var endpoint = Factory.Create<DeleteChoreEndpoint>(dbContextMock.Object);
            var request = new DeleteChoreRequest(1); // Non-existing chore id.

            // Act
            await endpoint.HandleAsync(request, default);
            var response = endpoint.Response;

            // Assert: The list remains empty and the endpoint response is null
            chores.Should().BeEmpty();
            endpoint.HttpContext.Response.StatusCode.Should().Be(404); // Not Found
        }
    }
}
