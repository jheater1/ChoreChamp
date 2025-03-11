namespace ChoreChamp.API.Features.Chores.UpdateChore;

public record UpdateChoreRequest(int Id, string Name, string Description, int Points);
