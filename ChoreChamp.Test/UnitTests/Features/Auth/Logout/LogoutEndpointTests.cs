using System.Threading;
using System.Threading.Tasks;
using ChoreChamp.API.Features.Auth.Logout;
using ChoreChamp.API.Shared.Constants;
using FastEndpoints;
using FastEndpoints.Security;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ChoreChamp.API.Tests.UnitTests.Features.Auth
{
    public class LogoutEndpointTests
    {
        [Fact]
        public async Task Logout_ReturnsNoContent()
        {
            // Arrange
            var endpoint = Factory.Create<LogoutEndpoint>(ctx =>
            {
                ctx.AddTestServices(s =>
                {
                    s.AddRouting();
                    s.AddAuthentication("Cookies").AddCookie("Cookies", options => { });
                    s.AddHttpContextAccessor();
                });
            });

            // Act
            await endpoint.HandleAsync(CancellationToken.None);

            // Assert: A successful logout should return 204 No Content.
            endpoint.HttpContext.Response.StatusCode.Should().Be(204);
        }
    }
}
