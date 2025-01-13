using Observability.Api;
using Observability.Api.Extensions;
using Observability.Application;
using Observability.Infrastructure;
using OpenTelemetry.Logs;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

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

app.Run();