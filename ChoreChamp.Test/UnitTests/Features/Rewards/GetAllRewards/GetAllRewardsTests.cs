using ChoreChamp.API.Features.Rewards.GetAllRewards;
using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.Test.UnitTests.Features.Rewards.Utilities;
using FastEndpoints;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace ChoreChamp.Test.UnitTests.Features.Rewards.GetAllRewards;

public class GetAllRewardsTests
{
    [Fact]
    public async Task GetAllRewards_WhenCalledWithExistingRewards_ReturnsAllRewards()
    {
        // Arrange: Create a list of rewards and build a mock DbSet from it.
        var mockRewards = RewardTestDataFactory.CreateRewards(3, true);
        var mockRewardDbSet = mockRewards.AsQueryable().BuildMockDbSet();
        // Create a mock for IApplicationDbContext.
        var dbContextMock = new Mock<IChoreChampDbContext>();
        dbContextMock.Setup(x => x.Rewards).Returns(mockRewardDbSet.Object);
        var endpoint = Factory.Create<GetAllRewardsEndpoint>(dbContextMock.Object);
        var request = new GetAllRewardsRequest();
        // Act
        await endpoint.HandleAsync(request, default);
        var response = endpoint.Response;
        // Assert
        response.Should().NotBeNull();
        response.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetAllRewards_WhenCalledWithUnavailableRewards_ReturnsAllRewards()
    {
        // Arrange: Create a list of rewards and build a mock DbSet from it.
        var mockRewards = RewardTestDataFactory.CreateRewards(3, true);
        var mockRewardDbSet = mockRewards.AsQueryable().BuildMockDbSet();
        // Create a mock for IApplicationDbContext.
        var dbContextMock = new Mock<IChoreChampDbContext>();
        dbContextMock.Setup(x => x.Rewards).Returns(mockRewardDbSet.Object);
        var endpoint = Factory.Create<GetAllRewardsEndpoint>(dbContextMock.Object);
        var request = new GetAllRewardsRequest(true);
        // Act
        await endpoint.HandleAsync(request, default);
        var response = endpoint.Response;
        // Assert
        response.Should().NotBeNull();
        response.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllRewards_WhenCalledWithNoRewards_ReturnsAnEmptyList()
    {
        // Arrange: Create an empty list of rewards and build a mock DbSet from it.
        var mockRewards = RewardTestDataFactory.CreateRewards(0);
        var mockRewardDbSet = mockRewards.AsQueryable().BuildMockDbSet();
        var dbContextMock = new Mock<IChoreChampDbContext>();
        dbContextMock.Setup(x => x.Rewards).Returns(mockRewardDbSet.Object);
        var endpoint = Factory.Create<GetAllRewardsEndpoint>(dbContextMock.Object);
        var request = new GetAllRewardsRequest();
        // Act
        await endpoint.HandleAsync(request, default);
        var response = endpoint.Response;
        // Assert
        response.Should().NotBeNull();
        response.Should().HaveCount(0);
    }
}
