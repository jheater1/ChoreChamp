using ChoreChamp.API.Infrastructure.Security;

namespace ChoreChamp.API.Domain;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Password Password { get; set; }
    public bool IsAdmin { get; set; }
    public Points Points { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    private User() { }

    public User(string name, string email, string rawPassword, bool isAdmin, IPasswordService passwordService)
    {
        Name = name;
        Email = email;
        IsAdmin = isAdmin;
        Points = new Points(0);
        CreatedAt = DateTime.UtcNow;
        Password = Password.Create(rawPassword, passwordService);
    }

    public void UpdatePassword(string oldPassword, string newPassword, IPasswordService passwordService)
    {
        if (!Password.Verify(oldPassword, passwordService))
            throw new UnauthorizedAccessException("Invalid password.");

        Password = Password.Create(newPassword, passwordService);
    }

    public bool VerifyPassword(string rawPassword, IPasswordService passwordService)
    {
        return Password.Verify(rawPassword, passwordService);
    }

    public void AddPoints(int points)
    {
        Points = Points.Add(points);
    }

    public void SubtractPoints(int points)
    {
        Points = Points.Subtract(points);
    }
}
