using ChoreChamp.API.Domain;
using ChoreChamp.API.Infrastructure.Persistence;
using MediatR;

namespace ChoreChamp.API.Features.CreateChore;

public class CreateChoreHandler(ChoreChampDbContext dbContext) : IRequestHandler<CreateChoreCommand, Chore>
{
    public async Task<Chore> Handle(CreateChoreCommand request, CancellationToken cancellationToken)
    {
        var chore = new Chore
        {
            Name = request.Name,
            Description = request.Description,
            Points = request.Points
        };
        dbContext.Chores.Add(chore);
        await dbContext.SaveChangesAsync(cancellationToken);
        return chore;
    }
}
