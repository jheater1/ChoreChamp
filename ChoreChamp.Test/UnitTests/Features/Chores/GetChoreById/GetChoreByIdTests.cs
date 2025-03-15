using System.Linq;
using ChoreChamp.API.Domain;
using ChoreChamp.API.Features.Chores.GetChorebyId;
using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.Test.UnitTests.Features.Chores.Utilities;
using FastEndpoints;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ChoreChamp.Test.UnitTests.Features.Chores.GetChoreById
{
    public class GetChoreByIdTests
    {
        [Fact]
        public async Task GetChoreById_WhenCalledWithExistingChore_ReturnsChore()
        {
            // Arrange: Create a list with one chore.
            var chores = ChoreTestDataFactory.CreateChores(1).ToList();
            var existingChore = chores.First();

            // Build a mock DbSet from the list.
            var mockChoreDbSet = chores.AsQueryable().BuildMockDbSet();

            // Create a mock context and set its Chores property.
            var dbContextMock = new Mock<IChoreChampDbContext>();
            dbContextMock.Setup(x => x.Chores).Returns(mockChoreDbSet.Object);

            // Setup FindAsync to return the correct chore when called.
            dbContextMock
                .Setup(x => x.Chores.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync(
                    (object[] ids) =>
                    {
                        var id = (int)ids[0];
                        return chores.FirstOrDefault(c => c.Id == id);
                    }
                );

            // Create the endpoint with the mocked context.
            var endpoint = Factory.Create<GetChoreByIdEndpoint>(dbContextMock.Object);
            var request = new GetChoreByIdRequest(existingChore.Id);

            // Act
            await endpoint.HandleAsync(request, default);
            var response = endpoint.Response;

            // Assert: Verify the endpoint response.
            response.Should().NotBeNull();
            response.Id.Should().Be(existingChore.Id);
            response.Name.Should().Be(existingChore.Name);
            response.Description.Should().Be(existingChore.Description);
            response.Points.Should().Be(existingChore.Points);
        }

        [Fact]
        public async Task GetChoreById_WhenCalledWithNonExistingChore_ReturnsNotFound()
        {
            // Arrange: Create an empty list of chores.
            var chores = ChoreTestDataFactory.CreateEmptyChores().ToList();
            var mockChoreDbSet = chores.AsQueryable().BuildMockDbSet();

            var dbContextMock = new Mock<IChoreChampDbContext>();
            dbContextMock.Setup(x => x.Chores).Returns(mockChoreDbSet.Object);

            var endpoint = Factory.Create<GetChoreByIdEndpoint>(dbContextMock.Object);
            var request = new GetChoreByIdRequest(1);

            // Act
            await endpoint.HandleAsync(request, default);

            // Assert: Expect a 404 status code.
            endpoint.HttpContext.Response.StatusCode.Should().Be(404);
        }
    }
}
