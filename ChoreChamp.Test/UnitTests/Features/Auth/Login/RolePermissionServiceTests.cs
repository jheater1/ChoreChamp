using ChoreChamp.API.Features.Auth.Login;
using ChoreChamp.API.Infrastructure.Security;
using FluentAssertions;

namespace ChoreChamp.API.Tests.UnitTests.Features.Auth
{
    public class RolePermissionServiceTests
    {
        [Fact]
        public void GetPermissionsForRoles_Admin_ReturnsExpectedPermissions()
        {
            // Arrange
            var service = new RolePermissionService();

            // Act
            var permissions = service.GetPermissionsForRoles(RoleNames.Admin);

            // Assert
            permissions.Should().NotBeNull();
            permissions.Should().Contain(PermissionNames.ChangePassword);
            permissions.Count().Should().Be(1);
        }

        [Fact]
        public void GetPermissionsForRoles_User_ReturnsExpectedPermissions()
        {
            // Arrange
            var service = new RolePermissionService();

            // Act
            var permissions = service.GetPermissionsForRoles(RoleNames.User);

            // Assert
            permissions.Should().NotBeNull();
            permissions.Should().Contain(PermissionNames.ChangePassword);
            permissions.Count().Should().Be(1);
        }

        [Fact]
        public void GetPermissionsForRoles_InvalidRole_ReturnsEmpty()
        {
            // Arrange
            var service = new RolePermissionService();

            // Act
            var permissions = service.GetPermissionsForRoles("NonExistentRole");

            // Assert
            permissions.Should().BeEmpty();
        }
    }
}
