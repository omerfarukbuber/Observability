using Microsoft.EntityFrameworkCore;
using Observability.Application.Abstractions.Authentication;
using Observability.Application.Abstractions.CQRS;
using Observability.Application.Abstractions.Data;
using Observability.Shared;

namespace Observability.Application.Users.Login;

public class LoginUserCommandHandler(IPasswordHasher hasher, IApplicationDbContext context) : ICommandHandler<LoginUserCommand>
{
    public async Task<Result> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken)
            .ContinueWith(task =>
            {
                var user = task.Result;
                if (user is null)
                {
                    return Result.Failure(Error.NotFound("User.NotFound", "User couldn't found."));
                }

                return !hasher.Verify(request.Password, user.PasswordHash)
                    ? Result.Failure(Error.Failure("Incorrect.Password",
                        "Email or password is wrong."))
                    : Result.Success;

            }, cancellationToken);
    }
}