using ChoreChamp.API.Domain;
using ChoreChamp.API.Infrastructure.Security;
using Moq;

namespace ChoreChamp.Test.UnitTests.Domain.Users;

public class UserTests
{
    private readonly Mock<IPasswordService> _passwordServicerMock;

    public UserTests()
    {
        _passwordServicerMock = new Mock<IPasswordService>();
    }

    [Fact]
    public void UpdatePassword_WhenOldPasswordIsCorrect_ShouldUpdate()
    {
        // Arrange
        string oldPassword = "OldPass123";
        string newPassword = "NewPass123";
        string hashedOldPassword = "hashed_old";
        string hashedNewPassword = "hashed_new";

        _passwordServicerMock.Setup(ph => ph.HashPassword(oldPassword)).Returns(hashedOldPassword);
        _passwordServicerMock.Setup(ph => ph.HashPassword(newPassword)).Returns(hashedNewPassword);
        _passwordServicerMock
            .Setup(ph => ph.VerifyPassword(oldPassword, hashedOldPassword))
            .Returns(true);

        var user = new User(
            "New User",
            "user@example.com",
            oldPassword,
            false,
            _passwordServicerMock.Object
        );

        // Act
        user.UpdatePassword(oldPassword, newPassword, _passwordServicerMock.Object);

        // Assert
        Assert.Equal(hashedNewPassword, user.Password.PasswordHash);
    }

    [Fact]
    public void UpdatePassword_WhenOldPasswordIsIncorrect_ShouldThrowException()
    {
        // Arrange
        string oldPassword = "OldPass123";
        string newPassword = "NewPass123";
        string hashedOldPassword = "hashed_old";

        _passwordServicerMock.Setup(ph => ph.HashPassword(oldPassword)).Returns(hashedOldPassword);
        _passwordServicerMock
            .Setup(ph => ph.VerifyPassword(It.IsAny<string>(), hashedOldPassword))
            .Returns(false);

        var user = new User(
            "New User",
            "user@example.com",
            oldPassword,
            false,
            _passwordServicerMock.Object
        );

        // Act & Assert
        var exception = Assert.Throws<UnauthorizedAccessException>(
            () => user.UpdatePassword("WrongOldPassword", newPassword, _passwordServicerMock.Object)
        );

        Assert.Equal("Invalid password.", exception.Message);
    }

    [Fact]
    public void UpdatePassword_WhenNewPasswordIsTooShort_ShouldThrowException()
    {
        // Arrange
        string oldPassword = "OldPass123";
        string newPassword = "short"; // Invalid password (too short)

        _passwordServicerMock.Setup(p => p.HashPassword(oldPassword)).Returns("hashed_old");
        _passwordServicerMock
            .Setup(ph => ph.VerifyPassword(oldPassword, It.IsAny<string>()))
            .Returns(true);

        var user = new User(
            "New User",
            "user@example.com",
            oldPassword,
            false,
            _passwordServicerMock.Object
        );

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => user.UpdatePassword(oldPassword, newPassword, _passwordServicerMock.Object)
        );

        Assert.Contains("Password must be between 8 and 64 characters.", exception.Message);
    }

    [Fact]
    public void UpdatePassword_WhenNewPasswordIsInvalid_ShouldThrowException()
    {
        // Arrange
        string oldPassword = "OldPass123";
        string newPassword = "thisisalongpassword"; // Invalid password (no numerical character)

        _passwordServicerMock.Setup(p => p.HashPassword(oldPassword)).Returns("hashed_old");
        _passwordServicerMock
            .Setup(ph => ph.VerifyPassword(oldPassword, It.IsAny<string>()))
            .Returns(true);

        var user = new User(
            "New User",
            "user@example.com",
            oldPassword,
            false,
            _passwordServicerMock.Object
        );

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => user.UpdatePassword(oldPassword, newPassword, _passwordServicerMock.Object)
        );

        Assert.Contains(
            "Password must contain at least one lowercase letter, one uppercase letter, and one number.",
            exception.Message
        );
    }

    [Fact]
    public void VerifyPassword_WhenPasswordIsCorrect_ShouldReturnTrue()
    {
        // Arrange
        string password = "Password1";
        string hashedPassword = "hashed_pass";

        _passwordServicerMock.Setup(ph => ph.HashPassword(password)).Returns(hashedPassword);
        _passwordServicerMock
            .Setup(ph => ph.VerifyPassword(password, hashedPassword))
            .Returns(true);

        var user = new User(
            "New User",
            "user@example.com",
            password,
            false,
            _passwordServicerMock.Object
        );

        // Act & Assert
        Assert.True(user.VerifyPassword(password, _passwordServicerMock.Object));
    }

    [Fact]
    public void VerifyPassword_WhenPasswordIsIncorrect_ShouldReturnFalse()
    {
        // Arrange
        string password = "Password1";
        string hashedPassword = "hashed_pass";

        _passwordServicerMock.Setup(ph => ph.HashPassword(password)).Returns(hashedPassword);
        _passwordServicerMock
            .Setup(ph => ph.VerifyPassword(password, hashedPassword))
            .Returns(false);

        var user = new User(
            "New User",
            "user@example.com",
            password,
            false,
            _passwordServicerMock.Object
        );

        // Act & Assert
        Assert.False(user.VerifyPassword("WrongPassword", _passwordServicerMock.Object));
    }

    [Fact]
    public void AddPoints_ShouldAddPoints()
    {
        // Arrange
        var password = "Password1";

        // Set up password service mock
        _passwordServicerMock.Setup(p => p.HashPassword(password)).Returns("hashed_password");
        _passwordServicerMock
            .Setup(ph => ph.VerifyPassword(password, It.IsAny<string>()))
            .Returns(true);

        // Set up user
        var user = new User(
            "New User",
            "user@example.com",
            password,
            false,
            _passwordServicerMock.Object
        );

        // Act
        user.AddPoints(10);

        // Assert
        Assert.Equal(10, user.Points.Value);
    }

    [Fact]
    public void SubtractPoints_ShouldSubtractPoints()
    {
        // Arrange
        var password = "Password1";

        // Set up password service mock
        _passwordServicerMock.Setup(p => p.HashPassword(password)).Returns("hashed_password");
        _passwordServicerMock
            .Setup(ph => ph.VerifyPassword(password, It.IsAny<string>()))
            .Returns(true);

        // Set up user
        var user = new User(
            "New User",
            "user@example.com",
            password,
            false,
            _passwordServicerMock.Object
        );

        // Act
        user.AddPoints(100);
        Assert.Equal(100, user.Points.Value);

        user.SubtractPoints(50);

        // Assert
        Assert.Equal(50, user.Points.Value);
    }
}
