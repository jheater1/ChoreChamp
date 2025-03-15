using System.Linq;
using ChoreChamp.API.Domain;
using ChoreChamp.API.Features.Chores.UpdateChore;
using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.Test.UnitTests.Features.Chores.Utilities;
using FastEndpoints;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ChoreChamp.Test.UnitTests.Features.Chores.UpdateChore
{
    public class UpdateChoreTests
    {
        [Fact]
        public async Task UpdateChore_WhenCalledWithExistingChore_UpdatesChore()
        {
            // Arrange: Create a list with one chore.
            var chores = ChoreTestDataFactory.CreateChores(1).ToList();
            var existingChore = chores.First();

            // Build a mock DbSet from the list.
            var mockChoreDbSet = chores.AsQueryable().BuildMockDbSet();

            // Set up Update so that when Update is called, it updates the underlying list.
            mockChoreDbSet
                .Setup(m => m.Update(It.IsAny<Chore>()))
                .Callback<Chore>(updatedChore =>
                {
                    var chore = chores.First(x => x.Id == updatedChore.Id);
                    chore.Name = updatedChore.Name;
                    chore.Description = updatedChore.Description;
                    chore.Points = updatedChore.Points;
                })
                .Returns((Chore updatedChore) => null);

            // Create a mock context and set its Chores property.
            var dbContextMock = new Mock<IChoreChampDbContext>();
            dbContextMock.Setup(x => x.Chores).Returns(mockChoreDbSet.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            // Create the endpoint with the mocked context.
            var endpoint = Factory.Create<UpdateChoreEndpoint>(dbContextMock.Object);
            var request = new UpdateChoreRequest(
                Id: existingChore.Id,
                Name: "Updated Name",
                Description: "Updated Description",
                Points: 10
            );

            // Act
            await endpoint.HandleAsync(request, default);
            var response = endpoint.Response;

            // Assert: Verify the underlying list was updated.
            var updatedChore = chores.First();
            updatedChore.Name.Should().Be("Updated Name");
            updatedChore.Description.Should().Be("Updated Description");
            updatedChore.Points.Should().Be(10);

            // And verify the endpoint response.
            response.Should().NotBeNull();
            response.Id.Should().Be(existingChore.Id);
            response.Name.Should().Be("Updated Name");
            response.Description.Should().Be("Updated Description");
            response.Points.Should().Be(10);
        }

        [Fact]
        public async Task UpdateChore_WhenCalledWithNonExistingChore_ReturnsNotFound()
        {
            // Arrange: Create an empty list of chores.
            var chores = ChoreTestDataFactory.CreateEmptyChores().ToList();
            var mockChoreDbSet = chores.AsQueryable().BuildMockDbSet();

            var dbContextMock = new Mock<IChoreChampDbContext>();
            dbContextMock.Setup(x => x.Chores).Returns(mockChoreDbSet.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(0);

            var endpoint = Factory.Create<UpdateChoreEndpoint>(dbContextMock.Object);
            var request = new UpdateChoreRequest(
                Id: 1,
                Name: "Non-Existing Chore",
                Description: "Non-Existing Description",
                Points: 10
            );

            // Act
            await endpoint.HandleAsync(request, default);

            // Assert: Expect a 404 status code.
            endpoint.HttpContext.Response.StatusCode.Should().Be(404);
        }
    }
}
