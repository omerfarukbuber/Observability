using Microsoft.EntityFrameworkCore;
using Observability.Api.Extensions;
using Observability.Application;
using Observability.Application.Abstractions.Data;
using Observability.Infrastructure;
using Observability.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.ApplyMigration();

app.Run();
