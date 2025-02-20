using Observability.Api;
using Observability.Api.Extensions;
using Observability.Application;
using Observability.Infrastructure;
using OpenTelemetry.Logs;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Host.ConfigureLogging();

builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapEndpoints();

app.UseSerilogRequestLogging();

app.ConfigureCustomResult();
app.ConfigureExceptionHandlers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    

    await app.ApplyMigration();
}
app.MapScalarApiReference();
app.Run();