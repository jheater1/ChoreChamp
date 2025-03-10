using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ChoreChamp.API.Domain;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool IsParent { get; set; }
    public int Points { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
