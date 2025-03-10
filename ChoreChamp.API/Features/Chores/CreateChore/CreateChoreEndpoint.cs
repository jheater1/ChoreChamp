using ChoreChamp.API.Domain;
using ChoreChamp.API.Infrastructure.Persistence;
using FastEndpoints;

namespace ChoreChamp.API.Features.Chores.CreateChore;

public class CreateChoreEndpoint : Endpoint<CreateChoreRequest, CreateChoreResponse, CreateChoreMapper>
{
    private readonly ChoreChampDbContext _dbContext;
    public CreateChoreEndpoint(ChoreChampDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("api/chores");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateChoreRequest r, CancellationToken c)
    {
        Chore chore = Map.ToEntity(r);
        _dbContext.Chores.Add(chore);
        await _dbContext.SaveChangesAsync(c);
        Response = Map.FromEntity(chore);
        await SendCreatedAtAsync<CreateChoreEndpoint>(new {id = chore.Id}, Response);
    }
}
