using Asp.Versioning;
using Observability.Api.Extensions;
using Observability.Api.Infrastructure;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection;

namespace Observability.Api;

internal static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddExceptionServices();
        services.AddOpenTelemetryServices();
        services.AddEndpoints(Assembly.GetExecutingAssembly());
        services.AddApiVersioning(builder =>
            {
                builder.DefaultApiVersion = new ApiVersion(1);
                builder.AssumeDefaultVersionWhenUnspecified = true;
                builder.ReportApiVersions = true;
                builder.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(builder =>
            {
                builder.GroupNameFormat = "'v'V";
                builder.SubstituteApiVersionInUrl = true;
            });

        services.AddHttpContextAccessor();
        services.AddOpenApi();

        return services;
    }

    private static IServiceCollection AddExceptionServices(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier);
            };
        });
        services.AddExceptionHandler<GlobalExceptionHandler>();
        return services;
    }

    private static IServiceCollection AddOpenTelemetryServices(this IServiceCollection services)
    {
        
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("Observability.Service"))
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation();
                metrics.AddOtlpExporter();
            })
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation(config =>
                {
                    config.EnrichWithHttpRequest = async (activity, request) =>
                    {
                        request.EnableBuffering();
                        var requestBodyStreamReader = new StreamReader(request.Body);
                        var requestBody = await requestBodyStreamReader.ReadToEndAsync();
                        activity.AddTag("http.request.body", requestBody);
                        request.Body.Position = 0;
                    };
                    config.EnrichWithException = (activity, exception) =>
                    {
                        activity.AddTag("exception.name", exception.GetType().Name);
                        activity.AddTag("exception.message", exception.Message);

                        if (exception.InnerException is null) return;
                        activity.AddTag("exception.inner-exception.name", exception.InnerException.GetType().Name);
                        activity.AddTag("exception.inner-exception.message", exception.InnerException.Message);
                    };
                });
                tracing.AddEntityFrameworkCoreInstrumentation(config =>
                {
                    config.SetDbStatementForText = true;
                    config.SetDbStatementForStoredProcedure = true;
                });
                tracing.AddRedisInstrumentation();

                tracing.AddOtlpExporter();
            });
        return services;
    }

    public static void ConfigureExceptionHandlers(this IApplicationBuilder app)
    {
        app.UseExceptionHandler();
    }
}