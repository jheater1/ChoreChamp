using ChoreChamp.API.Infrastructure.Security;

namespace ChoreChamp.API.Domain;

public class User
{
    public int Id { get; set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public Password Password { get; private set; }
    public bool IsAdmin { get; private set; }
    public Points Points { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private User() { }

    public User(
        string name,
        string email,
        string rawPassword,
        bool isAdmin,
        IPasswordService passwordService
    )
    {
        this.Name = name;
        this.Email = email;
        this.IsAdmin = isAdmin;
        this.Points = new Points(0);
        this.CreatedAt = DateTime.UtcNow;
        this.Password = Password.Create(rawPassword, passwordService);
    }

    public void UpdatePassword(
        string oldPassword,
        string newPassword,
        IPasswordService passwordService
    )
    {
        if (!Password.Verify(oldPassword, passwordService))
            throw new UnauthorizedAccessException("Invalid password.");

        this.Password = Password.Create(newPassword, passwordService);
    }

    public bool VerifyPassword(string rawPassword, IPasswordService passwordService)
    {
        return this.Password.Verify(rawPassword, passwordService);
    }

    public void AddPoints(int points)
    {
        this.Points = this.Points.Add(points);
    }

    public void SubtractPoints(int points)
    {
        this.Points = this.Points.Subtract(points);
    }
}
