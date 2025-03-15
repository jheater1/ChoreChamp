using ChoreChamp.API.Features.Chores.GetAllChores;
using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.Test.UnitTests.Features.Chores.Utilities;
using FastEndpoints;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace ChoreChamp.Test.UnitTests.Features.Chores.GetAllChores
{
    public class GetAllChoresTests
    {
        [Fact]
        public async Task GetAllChores_WhenCalledWithExistingChores_ReturnsAllChores()
        {
            // Arrange: Create a list of chores and build a mock DbSet from it.
            var mockChores = ChoreTestDataFactory.CreateChores(2);
            var mockChoreDbSet = mockChores.AsQueryable().BuildMockDbSet();

            // Create a mock for IApplicationDbContext.
            var dbContextMock = new Mock<IChoreChampDbContext>();
            dbContextMock.Setup(x => x.Chores).Returns(mockChoreDbSet.Object);

            var endpoint = Factory.Create<GetAllChoresEndpoint>(dbContextMock.Object);

            // Act
            await endpoint.HandleAsync(default);
            var response = endpoint.Response;

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllChores_WhenCalledWithNoChores_ReturnsAnEmptyList()
        {
            // Arrange: Create an empty list of chores and build a mock DbSet from it.
            var mockChores = ChoreTestDataFactory.CreateEmptyChores();
            var mockChoreDbSet = mockChores.AsQueryable().BuildMockDbSet();

            var dbContextMock = new Mock<IChoreChampDbContext>();
            dbContextMock.Setup(x => x.Chores).Returns(mockChoreDbSet.Object);

            var endpoint = Factory.Create<GetAllChoresEndpoint>(dbContextMock.Object);

            // Act
            await endpoint.HandleAsync(default);
            var response = endpoint.Response;

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(0);
        }
    }
}
