using ChoreChamp.API.Infrastructure.ApplicationConfiguration;
using ChoreChamp.API.Infrastructure.DependencyInjection;
using FastEndpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.ConfigureApp();

app.Run();
