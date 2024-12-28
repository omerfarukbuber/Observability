using MediatR;
using Microsoft.EntityFrameworkCore;
using Observability.Application.Abstractions.CQRS;
using Observability.Application.Abstractions.Data;
using Observability.Domain.Users;
using Observability.Shared;

namespace Observability.Application.Users.Register;

public class RegisterUserCommandHandler(IApplicationDbContext context) : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await context.Users.AnyAsync(u => u.Email.Equals(request.Email), cancellationToken))
        {
            return Result<Guid>.Failure(Error.Conflict("User.EmailNotUnique", "User email is not unique."));
        }

        
        
        return Result<Guid>.Success(Guid.NewGuid());
    }
}