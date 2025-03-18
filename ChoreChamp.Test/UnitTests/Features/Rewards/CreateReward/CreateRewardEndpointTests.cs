using ChoreChamp.API.Domain;
using ChoreChamp.API.Features.Rewards.CreateReward;
using ChoreChamp.API.Infrastructure.Persistence;
using FastEndpoints;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.Moq;
using Moq;

namespace ChoreChamp.Test.UnitTests.Features.Rewards.CreateReward;

public class CreateRewardEndpointTests
{
    [Fact]
    public async Task CreateReward_WhenCalledWithValidRequest_CreatesReward()
    {
        // Arrange
        var dbContextMock = new Mock<IChoreChampDbContext>();
        // Create an in-memory list to act as our DbSet
        var rewardList = new List<Reward>();
        // Convert the list into a mock DbSet that supports IQueryable operations
        var rewardDbSetMock = rewardList.AsQueryable().BuildMockDbSet();
        dbContextMock.Setup(x => x.Rewards).Returns(rewardDbSetMock.Object);
        // Setup SaveChangesAsync to return success
        dbContextMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);
        // Create the endpoint using the test factory and DI container
        var endpoint = Factory.Create<CreateRewardEndpoint>(ctx =>
        {
            ctx.AddTestServices(s =>
            {
                s.AddRouting();
                s.AddSingleton(dbContextMock.Object);
            });
        });
        var request = new CreateRewardRequest("New Reward", "Description", 10, null);
        // Act
        await endpoint.HandleAsync(request, default);
        var response = endpoint.Response;
        // Assert
        response.Should().NotBeNull();
        response.Name.Should().Be(request.Name);
        response.Description.Should().Be(request.Description);
        response.PointCost.Should().Be(request.PointCost);
        response.Limit.Should().Be(request.Limit);
        dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}
