using Observability.Application.Abstractions.CQRS;

namespace Observability.Application.Users.Login;

public sealed record LoginUserCommand(string Email, string Password) : ICommand;