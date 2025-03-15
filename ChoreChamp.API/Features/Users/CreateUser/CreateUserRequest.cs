namespace ChoreChamp.API.Features.Users.CreateUser;

public record CreateUserRequest(string Name, string Email, string Password, bool IsAdmin);
