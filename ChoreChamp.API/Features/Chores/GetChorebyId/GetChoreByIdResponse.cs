namespace ChoreChamp.API.Features.Chores.GetChorebyId;

public record GetChoreByIdResponse(int Id, string Name, string Description, int Points);
