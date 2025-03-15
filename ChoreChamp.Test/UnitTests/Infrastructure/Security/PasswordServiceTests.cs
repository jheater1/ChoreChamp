using ChoreChamp.API.Infrastructure.Security;
using FluentAssertions;
using Xunit;

namespace ChoreChamp.API.Tests.UnitTests.Infrastructure.Security
{
    public class PasswordServiceTests
    {
        private readonly PasswordService _passwordService = new();

        [Fact]
        public void HashPassword_ShouldReturnNonEmptyHash()
        {
            // Arrange
            var password = "password123";

            // Act
            var hash = _passwordService.HashPassword(password);

            // Assert
            hash.Should().NotBeNullOrEmpty();
            hash.Should().NotBe(password, "the hash should not match the plain password");
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
        {
            // Arrange
            var password = "password123";
            var hash = _passwordService.HashPassword(password);

            // Act
            var result = _passwordService.VerifyPassword(password, hash);

            // Assert
            result.Should().BeTrue("because the password matches its hash");
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
        {
            // Arrange
            var password = "password123";
            var hash = _passwordService.HashPassword(password);

            // Act
            var result = _passwordService.VerifyPassword("wrongpassword", hash);

            // Assert
            result.Should().BeFalse("because the wrong password does not match the hash");
        }
    }
}
