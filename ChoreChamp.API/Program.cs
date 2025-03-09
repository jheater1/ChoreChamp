using ChoreChamp.API.Features.CreateChore;
using ChoreChamp.API.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.MapCreateChoreEndpoints();

app.Run();
