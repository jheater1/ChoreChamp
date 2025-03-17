using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChoreChamp.API.Domain;
using ChoreChamp.API.Features.Auth.ChangePassword;
using ChoreChamp.API.Infrastructure.Persistence;
using ChoreChamp.API.Infrastructure.Security;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ChoreChamp.Test.UnitTests.Features.Auth.ChangePassword
{
    public class ChangePasswordEndpointTests
    {
        [Fact]
        public async Task ChangePassword_WithValidRequest_ChangesPassword()
        {
            // Arrange
            var passwordServiceMock = new Mock<IPasswordService>();
            // Verify current password check
            passwordServiceMock
                .Setup(p => p.VerifyPassword("oldPassword1", "oldHash"))
                .Returns(true);
            // Hash the new password
            passwordServiceMock.Setup(p => p.HashPassword("newPassword1")).Returns("newHash");
            passwordServiceMock.Setup(p => p.HashPassword("oldPassword1")).Returns("oldHash");

            var testUser = new User(
                "test",
                "test@test.com",
                "oldPassword1",
                false,
                passwordServiceMock.Object
            );

            var userList = new List<User> { testUser };
            var userDbSetMock = userList.AsQueryable().BuildMockDbSet();

            var dbContextMock = new Mock<IChoreChampDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(userDbSetMock.Object);

            var endpoint = Factory.Create<ChangePasswordEndpoint>(ctx =>
            {
                ctx.AddTestServices(s =>
                {
                    s.AddRouting();
                    s.AddSingleton(dbContextMock.Object);
                    s.AddSingleton(passwordServiceMock.Object);
                    s.AddAuthentication("Cookies").AddCookie("Cookies", options => { });
                    s.AddHttpContextAccessor();
                });
            });

            // Set a ClaimsPrincipal with the test user's email.
            endpoint.HttpContext.User = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[] { new Claim(ClaimTypes.Email, "test@test.com") },
                    "TestAuth"
                )
            );

            // Create a valid ChangePasswordRequest record.
            var request = new ChangePasswordRequest("oldPassword1", "newPassword1", "newPassword1");

            // Act
            await endpoint.HandleAsync(request, default);

            // Assert: Expect a 204 No Content response.
            endpoint.HttpContext.Response.StatusCode.Should().Be(204);

            // Verify that the password service hashed the new password.
            passwordServiceMock.Verify(p => p.HashPassword("newPassword1"), Times.Once);
        }

        [Fact]
        public async Task ChangePassword_WithMissingEmailClaim_ReturnsUnauthorized()
        {
            // Arrange
            var dbContextMock = new Mock<IChoreChampDbContext>();
            var userList = new List<User>(); // No users needed for this test.
            var userDbSetMock = userList.AsQueryable().BuildMockDbSet();
            dbContextMock.Setup(x => x.Users).Returns(userDbSetMock.Object);

            var passwordServiceMock = new Mock<IPasswordService>();

            var endpoint = Factory.Create<ChangePasswordEndpoint>(ctx =>
            {
                ctx.AddTestServices(s =>
                {
                    s.AddRouting();
                    s.AddSingleton(dbContextMock.Object);
                    s.AddSingleton(passwordServiceMock.Object);
                    s.AddAuthentication("Cookies").AddCookie("Cookies", options => { });
                    s.AddHttpContextAccessor();
                });
            });

            // Do not set any ClaimsPrincipal so that the email claim is missing.
            var request = new ChangePasswordRequest("oldPassword", "newPassword", "newPassword");

            // Act
            await endpoint.HandleAsync(request, default);

            // Assert: Expect unauthorized (401) due to missing email claim.
            endpoint.HttpContext.Response.StatusCode.Should().Be(401);
        }

        [Fact]
        public async Task ChangePassword_WithMismatchedPasswords_ReturnsBadRequest()
        {
            // Arrange
            var passwordServiceMock = new Mock<IPasswordService>();
            passwordServiceMock.Setup(p => p.HashPassword("oldPassword1")).Returns("oldHash");

            var testUser = new User(
                "Test",
                "test@test.com",
                "oldPassword1",
                false,
                passwordServiceMock.Object
            );

            var userList = new List<User> { testUser };
            var userDbSetMock = userList.AsQueryable().BuildMockDbSet();

            var dbContextMock = new Mock<IChoreChampDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(userDbSetMock.Object);

            var endpoint = Factory.Create<ChangePasswordEndpoint>(ctx =>
            {
                ctx.AddTestServices(s =>
                {
                    s.AddRouting();
                    s.AddSingleton(dbContextMock.Object);
                    s.AddSingleton(passwordServiceMock.Object);
                    s.AddAuthentication("Cookies").AddCookie("Cookies", options => { });
                    s.AddHttpContextAccessor();
                });
            });

            // Set a valid ClaimsPrincipal.
            endpoint.HttpContext.User = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[] { new Claim(ClaimTypes.Email, "test@test.com") },
                    "TestAuth"
                )
            );

            // Create a request with mismatched new password and confirmation.
            var request = new ChangePasswordRequest(
                "oldPassword1",
                "newPassword1",
                "differentPassword1"
            );

            // Act
            await endpoint.HandleAsync(request, default);

            // Assert: Expect a 400 Bad Request due to password mismatch.
            endpoint.HttpContext.Response.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task ChangePassword_WithInvalidCurrentPassword_ReturnsUnauthorized()
        {
            // Arrange
            var passwordServiceMock = new Mock<IPasswordService>();
            passwordServiceMock.Setup(p => p.HashPassword("oldPassword1")).Returns("oldHash");

            var testUser = new User(
                "Test",
                "test@test.com",
                "oldPassword1",
                false,
                passwordServiceMock.Object
            );

            var userList = new List<User> { testUser };
            var userDbSetMock = userList.AsQueryable().BuildMockDbSet();

            var dbContextMock = new Mock<IChoreChampDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(userDbSetMock.Object);

            var endpoint = Factory.Create<ChangePasswordEndpoint>(ctx =>
            {
                ctx.AddTestServices(s =>
                {
                    s.AddRouting();
                    s.AddSingleton(dbContextMock.Object);
                    s.AddSingleton(passwordServiceMock.Object);
                    s.AddAuthentication("Cookies").AddCookie("Cookies", options => { });
                    s.AddHttpContextAccessor();
                });
            });

            // Set the ClaimsPrincipal with the user's email.
            endpoint.HttpContext.User = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[] { new Claim(ClaimTypes.Email, "test@test.com") },
                    "TestAuth"
                )
            );

            // Create a request with an incorrect current password.
            var request = new ChangePasswordRequest(
                "wrongOldPassword1",
                "newPassword1",
                "newPassword1"
            );

            // Act
            await endpoint.HandleAsync(request, default);

            // Assert: Expect unauthorized (401) because the current password did not match.
            endpoint.HttpContext.Response.StatusCode.Should().Be(401);
        }
    }
}
