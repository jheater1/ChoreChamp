using System.Collections.Generic;
using System.Linq;
using ChoreChamp.API.Domain; // Assuming the User entity is defined here
using ChoreChamp.API.Features.Auth.Login;
using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Infrastructure.Security;
using ChoreChamp.API.Shared.Constants;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ChoreChamp.Test.UnitTests.Features.Auth.Login
{
    public class LoginEndpointTests
    {
        [Fact]
        public async Task Login_WithValidCredentials_ReturnsNoContent()
        {
            // Arrange
            var testUser = new User
            {
                Email = "test@test.com",
                PasswordHash = "hashed", // Assume this is the stored hash.
                IsAdmin = false,
            };

            // Create an in-memory list for Users and build a mock DbSet.
            var userList = new List<User> { testUser };
            var userDbSetMock = userList.AsQueryable().BuildMockDbSet();

            var dbContextMock = new Mock<IChoreChampDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(userDbSetMock.Object);

            // Set up the password service mock to validate the password.
            var passwordServiceMock = new Mock<IPasswordService>();
            passwordServiceMock.Setup(p => p.VerifyPassword("password", "hashed")).Returns(true);

            // Set up the role permission service mock.
            var rolePermissionServiceMock = new Mock<IRolePermissionService>();
            rolePermissionServiceMock
                .Setup(r => r.GetPermissionsForRoles(It.IsAny<string>()))
                .Returns(new List<string> { "perm1", "perm2" });

            // Create the endpoint using the test factory and configure authentication with scheme "Cookies".
            var endpoint = Factory.Create<LoginEndpoint>(ctx =>
            {
                ctx.AddTestServices(s =>
                {
                    s.AddRouting();
                    s.AddSingleton(dbContextMock.Object);
                    s.AddSingleton(passwordServiceMock.Object);
                    s.AddSingleton(rolePermissionServiceMock.Object);
                    // Register authentication services with the expected scheme "Cookies"
                    s.AddAuthentication("Cookies").AddCookie("Cookies", options => { });
                    s.AddHttpContextAccessor();
                });
            });

            // Instantiate the request record using positional parameters.
            var request = new LoginRequest("test@test.com", "password");

            // Act
            await endpoint.HandleAsync(request, default);

            // Assert: A successful login should result in a 204 No Content response.
            endpoint.HttpContext.Response.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            // Create an empty list for Users so that no user matches the credentials.
            var emptyUserList = new List<User>();
            var userDbSetMock = emptyUserList.AsQueryable().BuildMockDbSet();

            var dbContextMock = new Mock<IChoreChampDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(userDbSetMock.Object);

            // Set up the password service mock to return false.
            var passwordServiceMock = new Mock<IPasswordService>();
            passwordServiceMock
                .Setup(p => p.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            // Set up a dummy role permission service mock.
            var rolePermissionServiceMock = new Mock<IRolePermissionService>();

            // Create the endpoint using the test factory and configure authentication.
            var endpoint = Factory.Create<LoginEndpoint>(ctx =>
            {
                ctx.AddTestServices(s =>
                {
                    s.AddRouting();
                    s.AddSingleton(dbContextMock.Object);
                    s.AddSingleton(passwordServiceMock.Object);
                    s.AddSingleton(rolePermissionServiceMock.Object);
                    // Register authentication services with the expected scheme "Cookies"
                    s.AddAuthentication("Cookies").AddCookie("Cookies", options => { });
                    s.AddHttpContextAccessor();
                });
            });

            // Create a request record that doesn't match any user.
            var request = new LoginRequest("nonexistent@test.com", "wrongpassword");

            // Act
            await endpoint.HandleAsync(request, default);

            // Assert: An unsuccessful login should result in a 401 Unauthorized response.
            endpoint.HttpContext.Response.StatusCode.Should().Be(401);
        }
    }
}
