using ChoreChamp.API.Domain;
using ChoreChamp.API.Features.Rewards.GetRewardById;
using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.Test.UnitTests.Features.Rewards.Utilities;
using FastEndpoints;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace ChoreChamp.Test.UnitTests.Features.Rewards.GetRewardById;

public class GetRewardByIdTests
{
    [Fact]
    public async Task GetRewardById_WhenCalledWithExistingReward_ReturnsReward()
    {
        // Arrange: Create a list with one reward.
        var rewards = RewardTestDataFactory.CreateRewards(1).ToList();
        var existingReward = rewards.First();
        // Build a mock DbSet from the list.
        var mockRewardDbSet = rewards.AsQueryable().BuildMockDbSet();
        // Create a mock context and set its Rewards property.
        var dbContextMock = new Mock<IChoreChampDbContext>();
        dbContextMock.Setup(x => x.Rewards).Returns(mockRewardDbSet.Object);
        // Setup FindAsync to return the correct reward when called.
        dbContextMock
            .Setup(x => x.Rewards.FindAsync(It.IsAny<object[]>()))
            .ReturnsAsync(
                (object[] ids) =>
                {
                    var id = (int)ids[0];
                    return rewards.FirstOrDefault(c => c.Id == id);
                }
            );
        // Create the endpoint with the mocked context.
        var endpoint = Factory.Create<GetRewardByIdEndpoint>(dbContextMock.Object);
        var request = new GetRewardByIdRequest(existingReward.Id);
        // Act
        await endpoint.HandleAsync(request, default);
        var response = endpoint.Response;
        // Assert: Verify the endpoint response.
        response.Should().NotBeNull();
        response.Id.Should().Be(existingReward.Id);
        response.Name.Should().Be(existingReward.Name);
        response.Description.Should().Be(existingReward.Description);
        response.PointCost.Should().Be(existingReward.PointCost);
    }

    [Fact]
    public async Task GetRewardById_WhenCalledWithNonExistingReward_ReturnsNotFound()
    {
        // Arrange: Create an empty list of rewards.
        var rewards = RewardTestDataFactory.CreateRewards(0).ToList();
        var mockRewardDbSet = rewards.AsQueryable().BuildMockDbSet();
        // Create a mock context and set its Rewards property.
        var dbContextMock = new Mock<IChoreChampDbContext>();
        dbContextMock.Setup(x => x.Rewards).Returns(mockRewardDbSet.Object);
        // Create the endpoint with the mocked context.
        var endpoint = Factory.Create<GetRewardByIdEndpoint>(dbContextMock.Object);
        var request = new GetRewardByIdRequest(1);
        // Act
        await endpoint.HandleAsync(request, default);

        // Assert: Verify should return a 404 not found status code.
        endpoint.HttpContext.Response.StatusCode.Should().Be(404);
    }
}
