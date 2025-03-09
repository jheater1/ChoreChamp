using MediatR;

namespace ChoreChamp.API.Features.CreateChore;

public static class CreateChoreEndpoints
{
    public static void MapCreateChoreEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/chores", async (CreateChoreCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/chores/{result}", result);
        });
    }
}
