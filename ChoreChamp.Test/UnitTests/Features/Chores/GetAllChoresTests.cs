using ChoreChamp.API.Domain;
using ChoreChamp.API.Features.Chores.GetAllChores;
using ChoreChamp.API.Infrastructure.Persistence;
using FastEndpoints;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;

namespace ChoreChamp.API.Tests.UnitTests.Features.Chores;

public class GetAllChoresTests
{
    private ChoreChampDbContext CreateMockDbContext(IEnumerable<Chore> mockChores)
    {
        var choreChampContextMock = new Mock<ChoreChampDbContext>();
        choreChampContextMock.Setup(x => x.Chores).ReturnsDbSet(mockChores);

        return choreChampContextMock.Object;
    }

    [Fact]
    public async Task GetAllChores_WhenCalledWithExistingChores_ReturnsAllChores()
    {
        // Arrange
        var choreChampContextMock = CreateMockDbContext(ChoreTestDataFactory.CreateChores(2));
        var ep = Factory.Create<GetAllChoresEndpoint>(choreChampContextMock);

        // Act
        await ep.HandleAsync(default);
        var rsp = ep.Response;

        // Assert
        rsp.Should().NotBeNull();
        rsp.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllChores_WhenCalledWithNoChores_ReturnsAnEmptyList()
    {
        // Arrange
        var choreChampContextMock = CreateMockDbContext(ChoreTestDataFactory.CreateEmptyChores());
        var ep = Factory.Create<GetAllChoresEndpoint>(choreChampContextMock);

        // Act
        await ep.HandleAsync(default);
        var rsp = ep.Response;

        // Assert
        rsp.Should().NotBeNull();
        rsp.Should().HaveCount(0);
    }
}
