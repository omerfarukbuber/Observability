using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Observability.Application.Abstractions.Authentication;
using Observability.Application.Abstractions.CQRS;
using Observability.Application.Abstractions.Data;
using Observability.Domain.Users;
using Observability.Shared;

namespace Observability.Application.Users.Register;

public class RegisterUserCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher) : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await context.Users.AnyAsync(u => u.Email.Equals(request.Email), cancellationToken))
        {
            return Result<Guid>.Failure(Error.Conflict("User.EmailNotUnique", "User email is not unique."));
        }

        var user = new User
        {
            UserId = NewId.NextSequentialGuid(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = passwordHasher.Hash(request.Password)
        };

        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(user.UserId);
    }
}