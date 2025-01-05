using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Observability.Api.Endpoints;

namespace Observability.Api.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var serviceDescriptors = assembly.DefinedTypes.Where(type => 
            type is
        {
            IsAbstract: false,
            IsInterface: false
        } &&
            type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
        IEndpointRouteBuilder routeBuilder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(routeBuilder);
        }

        return app;
    }
}