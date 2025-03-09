using ChoreChamp.API.Domain;
using MediatR;

namespace ChoreChamp.API.Features.CreateChore;

public record CreateChoreCommand(string Name, string Description, int Points) : IRequest<Chore>;
