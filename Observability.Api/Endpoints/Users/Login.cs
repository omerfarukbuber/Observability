using MediatR;
using Observability.Api.Extensions;
using Observability.Api.Infrastructure;
using Observability.Application.Users.Login;

namespace Observability.Api.Endpoints.Users;

internal static class Login
{
    private sealed record Request(string Email, string Password);
    internal static RouteGroupBuilder LoginUserEndpoint(this RouteGroupBuilder app)
    {
        app.MapPost("/login",
            async (Request request, ISender sender, CancellationToken cancellationToken = default) =>
            {
                var command = new LoginUserCommand(request.Email, request.Password);
                var result = await sender.Send(command, cancellationToken);
                return result.Match(Results.NoContent, CustomResults.Problem);
            });

        return app;
    }
}