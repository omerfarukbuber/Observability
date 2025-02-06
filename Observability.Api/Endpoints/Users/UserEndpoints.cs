using Asp.Versioning;

namespace Observability.Api.Endpoints.Users;

internal class UserEndpoints : IEndpoint
{
    private const string BaseAddress = "api/v{apiVersion:apiVersion}/users";

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        app.MapGroup(BaseAddress)
            .RegisterUserEndpoint()
            .LoginUserEndpoint()
            .WithApiVersionSet(apiVersionSet);
    }
}