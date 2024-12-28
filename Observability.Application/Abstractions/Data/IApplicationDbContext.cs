using Microsoft.EntityFrameworkCore;
using Observability.Domain.Users;

namespace Observability.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    public DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}