namespace ChoreChamp.API.Features.Users.CreateUser;

public record CreateUserResponse(int Id, string Name, string Email, bool IsAdmin, int Points);
