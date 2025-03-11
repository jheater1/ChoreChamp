using ChoreChamp.API.Infrastructure.ApplicationConfiguration;
using ChoreChamp.API.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.Configure();

app.Run();
