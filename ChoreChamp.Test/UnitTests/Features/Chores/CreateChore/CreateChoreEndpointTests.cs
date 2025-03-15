using System.Collections.Generic;
using System.Linq;
using ChoreChamp.API.Domain;
using ChoreChamp.API.Features.Chores.CreateChore;
using ChoreChamp.API.Infrastructure.Persistence;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.Moq; // Ensure you include this using
using Moq;
using Xunit;

namespace ChoreChamp.Test.UnitTests.Features.Chores.CreateChore
{
    public class CreateChoreEndpointTests
    {
        [Fact]
        public async Task CreateChore_WhenCalledWithValidRequest_CreatesChore()
        {
            // Arrange
            var dbContextMock = new Mock<IChoreChampDbContext>();

            // Create an in-memory list to act as our DbSet
            var choreList = new List<Chore>();
            // Convert the list into a mock DbSet that supports IQueryable operations
            var choreDbSetMock = choreList.AsQueryable().BuildMockDbSet();
            dbContextMock.Setup(x => x.Chores).Returns(choreDbSetMock.Object);

            // Setup SaveChangesAsync to return success
            dbContextMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            // Create the endpoint using the test factory and DI container
            var endpoint = Factory.Create<CreateChoreEndpoint>(ctx =>
            {
                ctx.AddTestServices(s =>
                {
                    s.AddRouting();
                    s.AddSingleton(dbContextMock.Object);
                });
            });

            var request = new CreateChoreRequest("New Chore", "Description", 5);

            // Act
            await endpoint.HandleAsync(request, default);
            var response = endpoint.Response;

            // Assert
            response.Should().NotBeNull();
            response.Name.Should().Be(request.Name);
            response.Description.Should().Be(request.Description);
            response.Points.Should().Be(request.Points);
            dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }
}
