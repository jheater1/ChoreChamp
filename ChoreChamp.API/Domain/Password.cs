using System.Text.RegularExpressions;
using ChoreChamp.API.Infrastructure.Security;

namespace ChoreChamp.API.Domain;

public record Password
{
    private const int MinLength = 8;
    private const int MaxLength = 64;
    private static readonly Regex PasswordRegex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,64}$");

    public string PasswordHash { get; }

    private Password(string passwordHash)
    {
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
    }

    public static Password Create(string rawPassword, IPasswordService passwordService)
    {
        if (string.IsNullOrWhiteSpace(rawPassword))
            throw new ArgumentException("Password cannot be empty or whitespace.");

        if (rawPassword.Length < MinLength || rawPassword.Length > MaxLength)
            throw new ArgumentException(
                $"Password must be between {MinLength} and {MaxLength} characters."
            );

        if (!PasswordRegex.IsMatch(rawPassword))
            throw new ArgumentException(
                "Password must contain at least one lowercase letter, one uppercase letter, and one number."
            );

        string passwordHash = passwordService.HashPassword(rawPassword);
        return new Password(passwordHash);
    }

    public bool Verify(string rawPassword, IPasswordService passwordService)
    {
        return passwordService.VerifyPassword(rawPassword, PasswordHash);
    }
}
