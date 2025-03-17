using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChoreChamp.API.Domain;
using ChoreChamp.API.Features.Users.CreateUser;
using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Infrastructure.Security;
using ChoreChamp.Test;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ChoreChamp.API.Tests.UnitTests.Features.Users
{
    public class CreateUserEndpointTests
    {
        [Fact]
        public async Task CreateUser_WithValidRequest_CreatesUser()
        {
            // Arrange
            // Set up an empty in-memory list for Users.
            var users = new List<User>();
            var usersDbSetMock = users.AsQueryable().BuildMockDbSet();

            var dbContextMock = new Mock<IChoreChampDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(usersDbSetMock.Object);
            dbContextMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Simulate AddAsync by adding the new user to the list and setting an Id.
            dbContextMock
                .Setup(x => x.Users.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback<User, CancellationToken>(
                    (user, ct) =>
                    {
                        // Simulate EF Core generating an Id.
                        user.Id = 101;
                        users.Add(user);
                    }
                )
                .ReturnsAsync((User user, CancellationToken ct) => default);

            var passwordServiceMock = new Mock<IPasswordService>();
            // When hashing "secret", return a fake hash.
            passwordServiceMock.Setup(p => p.HashPassword("Password1")).Returns("hashedSecret");

            // Create the endpoint using FastEndpoints' test factory.
            var endpoint = Factory.Create<CreateUserEndpoint>(ctx =>
            {
                ctx.AddTestServices(s =>
                {
                    s.AddRouting();
                    s.AddSingleton(dbContextMock.Object);
                    s.AddSingleton(passwordServiceMock.Object);
                });
            });

            // Create a sample request using the record's positional parameters.
            var request = new CreateUserRequest("Test User", "test@example.com", "Password1", false);

            // Act
            await endpoint.HandleAsync(request, CancellationToken.None);
            var response = endpoint.Response;

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(101);
            response.Name.Should().Be("Test User");
            response.Email.Should().Be("test@example.com");
            response.IsAdmin.Should().BeFalse();
            // Assuming Points are defaulted to 0.
            response.Points.Should().Be(0);
            dbContextMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task CreateUser_WhenUserExists_ReturnsConflict()
        {
            // Arrange
            var passwordServiceMock = new Mock<IPasswordService>();
            passwordServiceMock.Setup(p => p.HashPassword("Password1")).Returns("oldHash");

            // Set up an in-memory list with an existing user having the same email.
            var existingUser = new User("Existing User", "test@example.com", "Password1", false, passwordServiceMock.Object);
            existingUser.AddPoints(10);

            var users = new List<User> { existingUser };
            var usersDbSetMock = users.AsQueryable().BuildMockDbSet();

            var dbContextMock = new Mock<IChoreChampDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(usersDbSetMock.Object);


            var endpoint = Factory.Create<CreateUserEndpoint>(ctx =>
            {
                ctx.AddTestServices(s =>
                {
                    s.AddRouting();
                    s.AddSingleton(dbContextMock.Object);
                    s.AddSingleton(passwordServiceMock.Object);
                });
            });

            // Create a request that uses the same email as the existing user.
            var request = new CreateUserRequest("New User", "test@example.com", "Password1", false);

            // Act
            await endpoint.HandleAsync(request, CancellationToken.None);

            // Assert: Expect a 409 Conflict because a user with the same email exists.
            endpoint.HttpContext.Response.StatusCode.Should().Be(409);
        }
    }
}
