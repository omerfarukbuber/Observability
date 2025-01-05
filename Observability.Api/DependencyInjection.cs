using Asp.Versioning;
using Observability.Api.Extensions;
using Observability.Api.Infrastructure;
using System.Reflection;

namespace Observability.Api;

internal static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddExceptionServices();
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

    public static void ConfigureExceptionHandlers(this IApplicationBuilder app)
    {
        app.UseExceptionHandler();
    }
}