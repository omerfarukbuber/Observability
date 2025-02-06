using MediatR;
using Microsoft.Identity.Client;
using Observability.Api.Extensions;
using Observability.Api.Infrastructure;
using Observability.Application.Users.Register;

namespace Observability.Api.Endpoints.Users;

internal static class Register
{
    private sealed record Request(string Email, string FirstName, string LastName, string Password);
    internal static RouteGroupBuilder RegisterUserEndpoint(this RouteGroupBuilder app)
    {
        app.MapPost("/register",
            async (Request request, ISender sender, CancellationToken cancellationToken = default) =>
            {
                var command = new RegisterUserCommand(request.Email,
                    request.FirstName,
                    request.LastName,
                    request.Password);

                var result = await sender.Send(command, cancellationToken);
                return result.Match(Results.Ok, CustomResults.Problem);
            });

        return app;
    }
}